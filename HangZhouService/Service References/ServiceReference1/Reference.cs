﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HangZhouService.ServiceReference1 {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.yServiceSoap")]
    public interface yServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetData(string wbNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetDataAsync(string wbNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PutData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void PutData(string wbNo, string opEr, string opType, System.DateTime opTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PutData", ReplyAction="*")]
        System.Threading.Tasks.Task PutDataAsync(string wbNo, string opEr, string opType, System.DateTime opTime);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface yServiceSoapChannel : HangZhouService.ServiceReference1.yServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class yServiceSoapClient : System.ServiceModel.ClientBase<HangZhouService.ServiceReference1.yServiceSoap>, HangZhouService.ServiceReference1.yServiceSoap {
        
        public yServiceSoapClient() {
        }
        
        public yServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public yServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public yServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public yServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet GetData(string wbNo) {
            return base.Channel.GetData(wbNo);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetDataAsync(string wbNo) {
            return base.Channel.GetDataAsync(wbNo);
        }
        
        public void PutData(string wbNo, string opEr, string opType, System.DateTime opTime) {
            base.Channel.PutData(wbNo, opEr, opType, opTime);
        }
        
        public System.Threading.Tasks.Task PutDataAsync(string wbNo, string opEr, string opType, System.DateTime opTime) {
            return base.Channel.PutDataAsync(wbNo, opEr, opType, opTime);
        }
    }
}
