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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.yServiceSoap")]
    public interface yServiceSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 logistics_No 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetInfo", ReplyAction="*")]
        HangZhouService.ServiceReference1.GetInfoResponse GetInfo(HangZhouService.ServiceReference1.GetInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetInfo", ReplyAction="*")]
        System.Threading.Tasks.Task<HangZhouService.ServiceReference1.GetInfoResponse> GetInfoAsync(HangZhouService.ServiceReference1.GetInfoRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 logistics_No 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SetExam", ReplyAction="*")]
        HangZhouService.ServiceReference1.SetExamResponse SetExam(HangZhouService.ServiceReference1.SetExamRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SetExam", ReplyAction="*")]
        System.Threading.Tasks.Task<HangZhouService.ServiceReference1.SetExamResponse> SetExamAsync(HangZhouService.ServiceReference1.SetExamRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetInfoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetInfo", Namespace="http://tempuri.org/", Order=0)]
        public HangZhouService.ServiceReference1.GetInfoRequestBody Body;
        
        public GetInfoRequest() {
        }
        
        public GetInfoRequest(HangZhouService.ServiceReference1.GetInfoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetInfoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string logistics_No;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string app_No;
        
        public GetInfoRequestBody() {
        }
        
        public GetInfoRequestBody(string logistics_No, string app_No) {
            this.logistics_No = logistics_No;
            this.app_No = app_No;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetInfoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetInfoResponse", Namespace="http://tempuri.org/", Order=0)]
        public HangZhouService.ServiceReference1.GetInfoResponseBody Body;
        
        public GetInfoResponse() {
        }
        
        public GetInfoResponse(HangZhouService.ServiceReference1.GetInfoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetInfoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetInfoResult;
        
        public GetInfoResponseBody() {
        }
        
        public GetInfoResponseBody(string GetInfoResult) {
            this.GetInfoResult = GetInfoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SetExamRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SetExam", Namespace="http://tempuri.org/", Order=0)]
        public HangZhouService.ServiceReference1.SetExamRequestBody Body;
        
        public SetExamRequest() {
        }
        
        public SetExamRequest(HangZhouService.ServiceReference1.SetExamRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SetExamRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string logistics_No;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string app_No;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int checkWay;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int checkResult;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public int inspectionStatus;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string result;
        
        public SetExamRequestBody() {
        }
        
        public SetExamRequestBody(string logistics_No, string app_No, int checkWay, int checkResult, int inspectionStatus, string result) {
            this.logistics_No = logistics_No;
            this.app_No = app_No;
            this.checkWay = checkWay;
            this.checkResult = checkResult;
            this.inspectionStatus = inspectionStatus;
            this.result = result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SetExamResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SetExamResponse", Namespace="http://tempuri.org/", Order=0)]
        public HangZhouService.ServiceReference1.SetExamResponseBody Body;
        
        public SetExamResponse() {
        }
        
        public SetExamResponse(HangZhouService.ServiceReference1.SetExamResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SetExamResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SetExamResult;
        
        public SetExamResponseBody() {
        }
        
        public SetExamResponseBody(string SetExamResult) {
            this.SetExamResult = SetExamResult;
        }
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
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HangZhouService.ServiceReference1.GetInfoResponse HangZhouService.ServiceReference1.yServiceSoap.GetInfo(HangZhouService.ServiceReference1.GetInfoRequest request) {
            return base.Channel.GetInfo(request);
        }
        
        public string GetInfo(string logistics_No, string app_No) {
            HangZhouService.ServiceReference1.GetInfoRequest inValue = new HangZhouService.ServiceReference1.GetInfoRequest();
            inValue.Body = new HangZhouService.ServiceReference1.GetInfoRequestBody();
            inValue.Body.logistics_No = logistics_No;
            inValue.Body.app_No = app_No;
            HangZhouService.ServiceReference1.GetInfoResponse retVal = ((HangZhouService.ServiceReference1.yServiceSoap)(this)).GetInfo(inValue);
            return retVal.Body.GetInfoResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<HangZhouService.ServiceReference1.GetInfoResponse> HangZhouService.ServiceReference1.yServiceSoap.GetInfoAsync(HangZhouService.ServiceReference1.GetInfoRequest request) {
            return base.Channel.GetInfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<HangZhouService.ServiceReference1.GetInfoResponse> GetInfoAsync(string logistics_No, string app_No) {
            HangZhouService.ServiceReference1.GetInfoRequest inValue = new HangZhouService.ServiceReference1.GetInfoRequest();
            inValue.Body = new HangZhouService.ServiceReference1.GetInfoRequestBody();
            inValue.Body.logistics_No = logistics_No;
            inValue.Body.app_No = app_No;
            return ((HangZhouService.ServiceReference1.yServiceSoap)(this)).GetInfoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HangZhouService.ServiceReference1.SetExamResponse HangZhouService.ServiceReference1.yServiceSoap.SetExam(HangZhouService.ServiceReference1.SetExamRequest request) {
            return base.Channel.SetExam(request);
        }
        
        public string SetExam(string logistics_No, string app_No, int checkWay, int checkResult, int inspectionStatus, string result) {
            HangZhouService.ServiceReference1.SetExamRequest inValue = new HangZhouService.ServiceReference1.SetExamRequest();
            inValue.Body = new HangZhouService.ServiceReference1.SetExamRequestBody();
            inValue.Body.logistics_No = logistics_No;
            inValue.Body.app_No = app_No;
            inValue.Body.checkWay = checkWay;
            inValue.Body.checkResult = checkResult;
            inValue.Body.inspectionStatus = inspectionStatus;
            inValue.Body.result = result;
            HangZhouService.ServiceReference1.SetExamResponse retVal = ((HangZhouService.ServiceReference1.yServiceSoap)(this)).SetExam(inValue);
            return retVal.Body.SetExamResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<HangZhouService.ServiceReference1.SetExamResponse> HangZhouService.ServiceReference1.yServiceSoap.SetExamAsync(HangZhouService.ServiceReference1.SetExamRequest request) {
            return base.Channel.SetExamAsync(request);
        }
        
        public System.Threading.Tasks.Task<HangZhouService.ServiceReference1.SetExamResponse> SetExamAsync(string logistics_No, string app_No, int checkWay, int checkResult, int inspectionStatus, string result) {
            HangZhouService.ServiceReference1.SetExamRequest inValue = new HangZhouService.ServiceReference1.SetExamRequest();
            inValue.Body = new HangZhouService.ServiceReference1.SetExamRequestBody();
            inValue.Body.logistics_No = logistics_No;
            inValue.Body.app_No = app_No;
            inValue.Body.checkWay = checkWay;
            inValue.Body.checkResult = checkResult;
            inValue.Body.inspectionStatus = inspectionStatus;
            inValue.Body.result = result;
            return ((HangZhouService.ServiceReference1.yServiceSoap)(this)).SetExamAsync(inValue);
        }
    }
}
