﻿#region Using directives
using System.Runtime.Serialization;
#endregion

namespace treeDiM.StackBuilder.WCFAppServ
{
    [DataContract]
    public class DCSBLoadResultSingle
    {
        [DataMember]
        public DCSBStatus Status { get; set; }
        [DataMember]
        public DCCompFileOutput OutFile { get; set; }
    }

    [DataContract]
    public class DCSBLoadResultSinglePallet : DCSBLoadResultSingle
    {
        [DataMember]
        public DCSBLoadResultPallet Result { get; set; }
        //[DataMember]
        //public DCSBSuggest[] Suggestions { get; set; }
        [DataMember]
        public DCSBSuggestIncreasePalletXY SuggestPalletLength { get; set;}
        [DataMember]
        public DCSBSuggestIncreasePalletXY SuggestPalletWidth { get; set; }
        [DataMember]
        public DCSBSuggestIncreasePalletXY SuggestPalletDim { get; set; }
        [DataMember]
        public DCSBSuggestIncreasePalletZ SuggestPalletHeight { get; set; }
        [DataMember]
        public DCSBSuggestDecreaseCaseXY SuggestCaseDim1 { get; set; }
        [DataMember]
        public DCSBSuggestDecreaseCaseXY SuggestCaseDim2 { get; set; }

    }
    [DataContract]
    public class DCSBLoadResultSingleContainer : DCSBLoadResultSingle
    {
        [DataMember]
        public DCSBLoadResultContainer Result { get; set; }
    }
}