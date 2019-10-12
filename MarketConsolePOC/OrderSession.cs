using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.Fields;

namespace FixOrderConsole
{
    public class OrderSession : QuickFix.MessageCracker, QuickFix.IApplication
    {
        #region "Variable Declare"
        public Session _session;
        public EventHandler ClientNotifyLogoff;
        public OrdStatus ordStatus;
        public IInitiator MyInitiator = null;
        #endregion

        //FixDispatcher tradeDispatcherClient = new FixDispatcher();
        #region IApplication interface overrides
        public OrderSession()
        {
        }
        public void OnCreate(SessionID sessionID)
        {
            _session = Session.LookupSession(sessionID);
        }
        //public static Session session
        //{ get; set; }
        public void OnLogon(SessionID sessionID) { Console.WriteLine("Received Logon - " + sessionID.ToString()); }
        public void OnLogout(SessionID sessionID)
        {
            Console.WriteLine("Received Logout - " + sessionID.ToString());
            if (ClientNotifyLogoff != null)
                ClientNotifyLogoff.Invoke(this, null);
        }

        public void FromAdmin(Message message, SessionID sessionID)
        {

            if (message.Header.GetString(35) == QuickFix.FIX43.Reject.MsgType)
            {

            }
            else
            {
                Console.WriteLine("## FromAdmin: " + message.ToString());
            }

        }

        internal void GenerateLogout()
        {
            _session.GenerateLogout();
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
            try
            {
                //Crack(message,sessionID);
                if (message.GetType() == typeof(QuickFix.FIX43.Logon))
                {
                    message.SetField(new Username("bford"));
                    message.SetField(new Password("Faldaxadmin123"));
                }

                Console.WriteLine("#ToAdmin==" + message.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine("#ERRORToADMIN==" + message.ToString() + "/n====ERROR==" + e.ToString());
            }
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            try
            {
                Console.WriteLine("From app message :\n" + message.ToString());
                Crack(message, sessionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("==Cracker exception==");
                if (message.GetField(58) != null)
                    Console.WriteLine(message.GetField(58).ToString());
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
            }
        }
        public void ToApp(Message message, SessionID sessionID)
        {
            try
            {
                bool possDupFlag = false;
                if (message.Header.IsSetField(QuickFix.Fields.Tags.PossDupFlag))
                {
                    possDupFlag = QuickFix.Fields.Converters.BoolConverter.Convert(
                        message.Header.GetField(QuickFix.Fields.Tags.PossDupFlag)); /// FIXME
                }
                if (possDupFlag)
                    throw new DoNotSend();
            }
            catch (FieldNotFoundException)
            { }

            //Console.WriteLine();
            //Console.WriteLine("OUT: " + message.ToString());
        }
        #endregion


        #region MessageCracker handlers

        public void OnMessage(QuickFix.FIX43.ExecutionReport m, SessionID s)
        {
            string orderId = m.ClOrdID.getValue();
            //== checking that order is availble in orderprocessor or not =================
            var receiveOrder = OrderHelper.OrderProcessors.FirstOrDefault(x => x.OrderNo == orderId);
            if (receiveOrder == null)
            {
                Console.WriteLine($"No Existing order available for Orderid : {orderId}");
                return;
            }
            //============================================================================

            //=== compare received execution report status ==============================
            //Valid Responses  Execution Report(#35=8)

            receiveOrder.executionReport = m;

            if (receiveOrder.createOrderResponse == null)
                receiveOrder.createOrderResponse = new ViewModel.CreateOrderResponse();
            var r = receiveOrder.createOrderResponse;


            r.ClOrdID = orderId;
            r.ExecID = m.ExecID.getValue();
            r.ExecInst = m.ExecInst.getValue();
            r.OrigClOrd = m.ExecInst.getValue();
            r.ExecType = m.ExecType.getValue();
            r.OrdStatus = m.OrdStatus.getValue();
            if (r.OrdStatus != '8')
            {
                r.Symbol = m.Symbol.getValue();
                r.Side = m.Side.getValue();
                r.OrderQty = m.OrderQty.getValue();
                r.OrdType = m.OrdType.getValue();
                if(r.OrdType == '2')   r.Price = m.Price.getValue();
                r.Currency = m.Currency.getValue();
                r.SecurityType = m.SecurityType.getValue();
                r.TimeInForce = m.TimeInForce.getValue();
                r.LastPx = m.LastPx.getValue();
                r.LastQty = m.LastQty.getValue();
                r.AvgPx = m.AvgPx.getValue();
                r.TransactTime = m.TransactTime.getValue();
                //if(m.MinQty != null)  r.MinQty = m.MinQty.getValue();
               // r.ExpireTime = m.ExpireTime.getValue();
              //  r.MaxShow = m.MaxShow.getValue();
                r.Product = m.Product.getValue();
            }

            //Console.WriteLine($"Received execution report {m.ToString()}");
            if (r.OrdStatus != '0')
            {
                Enums.OrderStatus rstatus = (Enums.OrderStatus)((byte)r.OrdStatus);
                Console.WriteLine($"Order {r.ClOrdID} Status is {rstatus.ToString()}");
                receiveOrder.SetForOrderExecution();
            }
            else
            {
                Console.WriteLine($"Order {r.ClOrdID} Status is New.. wait for next message");
            }
            


            //byte ordStatus =
            //if (ordStatus == Enums.OrderStatus.REJECTED)
            //{

            //}

            //===========================================================================





        }




        public void OnMessage(QuickFix.FIX43.OrderCancelReject m, SessionID s)
    {
        Console.WriteLine("Received order cancel reject");
    }


    public void onMessage(QuickFix.FIX43.Logout message_, SessionID session_)
    {

        Text text = new Text();
        if (message_.IsSetText())
        {
            message_.Get(text);
            string logoutMessage = text.ToString();
            if (logoutMessage.IndexOf("Low sequence number ") > -1)
            {
                //message sequence number we are sending is too low 
                int index = logoutMessage.IndexOf("expecting");
                if (index > -1)
                {
                    int nextSpace = logoutMessage.LastIndexOf(' ');
                    if (nextSpace > -1)
                    {
                        int expectedNum = Convert.ToInt32(logoutMessage.Substring(nextSpace + 1));
                        Session.LookupSession(session_).NextSenderMsgSeqNum = expectedNum;
                    }
                }
            }
        }
    }

    public void SendMessage(Message m)
    {
        bool blnSendMessage = false;
        if (_session != null)
        {
            blnSendMessage = _session.Send(m);
            Console.WriteLine($"Send Message {blnSendMessage}");
        }
        else
        {
            // This probably won't ever happen.
            Console.WriteLine("Can't send message: session not created.");
        }
    }


    #endregion

}
}
