using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.Fields;

namespace MarketConsolePOC
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
            byte ordStatus = (byte)m.OrdStatus.getValue();
            if(ordStatus == Enums.OrderStatus.REJECTED)
            {

            }

            Console.WriteLine($"Received execution report {m.ToString()}");
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
