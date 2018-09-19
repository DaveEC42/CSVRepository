//https://davebochichio.com
//https://github.com/DaveEC42
//
//Credit: Deborah Kurata for providing this class via Pluralsight. 
//    https://blogs.msmvps.com/deborahk/about-me/
//
//
using System.Collections.Generic;

namespace BasicCSVRepository
{
    /// <summary>
    /// This class functions as means to provide a success/fail on any given operation, along
    /// with messages regarding the success/fail. This is an alternative to try/catch blocks,
    /// where the error that occurred is non-critical, the application is to continue execution,
    /// but validation is required and/or messages are to be returned to the calling code.
    /// </summary>
    public class OperationResult
    {
        public bool Success = true;
        public List<string> Messages;

        public OperationResult()
        {
            Messages = new List<string>();
        }

    }


}


