//using System;


//namespace GridProteinFolding.Logging.Exception
//{
//    /// <summary>
//    /// Summary description for CustomException
//    /// </summary>
//    public class CustomException : ApplicationException
//    {
//        // private members 
//        // defines the severity level of the Exception
//        private SeverityLevel severityLevelOfException;
//        // defines the logLevel of the Exception
//        private LogLevel logLevelOfException;
//        // System Exception that is thrown 
//        private System.Exception innerException;
//        // Custom Message 
//        private string customMessage;
//        /// <summary>
//        /// Public accessor of customMessage
//        /// </summary>
//        public string CustomMessage
//        {
//            get { return this.customMessage; }
//            set { this.customMessage = value; }
//        }
//        /// <summary>
//        /// Standard default Constructor
//        /// </summary>
//        internal CustomException()
//        { }
//        /// <summary>
//        /// Constructor with parameters 
//        /// </summary>
//        /// <param name="severityLevel"></param>
//        /// <param name="logLevel"></param>
//        /// <param name="exception"></param>
//        /// <param name="customMessage"></param> 
//        internal CustomException(SeverityLevel severityLevel, LogLevel logLevel, System.Exception exception, string customMessage)
//        {
//            this.severityLevelOfException = severityLevel;
//            this.logLevelOfException = logLevel;
//            this.innerException = exception;
//            this.customMessage = customMessage;
//        }
//    }

//}
