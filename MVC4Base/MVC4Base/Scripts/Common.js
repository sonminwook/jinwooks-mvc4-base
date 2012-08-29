function Common() {

    //** 브라우저명
    var m_AppName = navigator.appName;

    //** 브라우저 버전
    var m_AppVersion = parseFloat(navigator.appVersion.split("MSIE")[1]);

    // 기본 웹루트 입니다.
    var m_WebRoot = '/Main';

    // 메세지 팝업창의 타입을 지정한다.
    var m_MessageType = "normal";

    function fn_GetMessageType() {

        if (document.getElementById("hidFrameworkMessageType") != null) {
            m_MessageType = document.getElementById("hidFrameworkMessageType").value
        }
    }

    //alert(m_AppName);
    //alert(m_AppVersion);
    //alert(navigator.appVersion);
    //alert(navigator.appCodeName);
    //alert(navigator.userAgent);

    /************************************************************************
    함수명		: fn_ParseXmlMessage
    작성목적	: 메시지 XML 함수에서 정의한 메시지 Return
    Parameter 	: messageID : 오류번호 또는 오류내용
    Return		: 메세지
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    function fn_ParseXmlMessage(messageID) {

        //alert(m_AppName);

        var strCurture = "ko-KR"; // 기본 언어는 한국어임.

        var hidCurtureID = document.getElementById("hidCurtureID");

        if (hidCurtureID != null) {
            strCurture = hidCurtureID.value;
        }

        //alert(strCurture);

        var regBool = false;
        var xmlMsg = messageID;

        try {

            // 공통코드에서 조회가 가능한지 확인한다.
            if (!(/[a-zA-Z_]{2,2}[a-zA-Z_]{1,1}\d{3,3}$/.test(messageID))) {
                regBool = false;
            }
            else {
                regBool = true;
            }

            // 오류코드 검색인지 확인하고 그렇다면 코드로 검색한다.
            if (regBool) {

                //var XmlUrl = m_WebRoot + "/WebCommon/xml/Message.xml";
                var XmlUrl = "/WebCommon/xml/Message.xml";

                // IE에서는 아래의 코딩을 사용한다.
                if (window.ActiveXObject) {

                    var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
                    xmlDoc.async = "false";
                    xmlDoc.load(XmlUrl);
                }
                else {

                    // 파이어폭스, 크롬, 사파리에서는 아래의 코드를 사용한다.
                    xmlRequest = new XMLHttpRequest();
                    xmlRequest.overrideMimeType('text/xml');

                    xmlRequest.open("GET", XmlUrl, false);
                    xmlRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                    xmlRequest.send(null);

                    var parser = new DOMParser();
                    var xmlDoc = parser.parseFromString(xmlRequest.responseText, "text/xml");
                }

                var root = xmlDoc.getElementsByTagName('messages')[0];
                var items = root.getElementsByTagName("message");

                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    var code = item.getAttribute("code");
                    var curture = item.getAttribute("culture");

                    if (messageID == code && strCurture == curture) {
                        xmlMsg = item.firstChild.nodeValue;
                    }
                }
            }

            return xmlMsg;

        }
        catch (exception) {
            alert("시스템 오류입니다. " + exception.description);
        }
    }

    /************************************************************************
    함수명		: fn_GetWidth
    작성목적	: 팝업창 실행시 IE의 종류에 따라 넓이를 계산해 준다.
    Parameter	: width -> 
    Return		: 브라우져 버전에 따른 창의 크기 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    function fn_GetWidth(width) {

        var iWidth;
        iWidth = width;

        // IE6, 5.5 이면 +8을 한다.
        if ((m_AppName == "Microsoft Internet Explorer") && (m_AppVersion < 7)) {
            iWidth = iWidth + 8;
        }

        return iWidth;
    }

    /************************************************************************
    함수명		: fn_GetHeight
    작성목적	: 팝업창 실행시 IE의 종류에 따라 높이를 계산해 준다.
    Parameter	: height -> 
    Return		: IE에 따라 창의 크기 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    function fn_GetHeight(height) {

        var iHeight;
        iHeight = height;


        // IE6,5.5 이면 +40을 한다.
        if ((m_AppName == "Microsoft Internet Explorer") && (m_AppVersion < 7)) {
            iHeight = iHeight + 40;
        }


        return iHeight;
    }

    /************************************************************************
    함수명		: ErrorMessage
    작성목적	: 에러 메시지 상자를 띠운다.
    Parameter	: message - 코드 or 출력할 메시지
    Return		: alert으로 에러메세지 출력
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.ErrorMessage = function fn_ErrorMessage(message) {
        try {

            var xmlMsg;
            xmlMsg = fn_ParseXmlMessage(message);
            xmlMsg = "* [!]오류\r\n---------------------------------------------------\r\n " + xmlMsg

            alert(xmlMsg);
        }
        catch (exception) {
            alert("시스템 오류입니다. " + exception.description);
        }
    }

    /************************************************************************
    함수명		: Neoplus.ErrorMessage
    작성목적	: 에러 메시지 상자를 띠운다.
    Parameter	: message - 코드 or 출력할 메시지
    addInfo - 추가 정보
    Return		: showModalDialog로 에러메세지 출력
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.NeoplusErrorMessage = function fn_NeoplusErrorMessage(message, addInfo) {

        fn_GetMessageType();
        if (m_MessageType == 'normal') {
            Common.ErrorMessage(message);
        }
        else {

            var xmlMsg;
            xmlMsg = fn_ParseXmlMessage(message);

            // 추가정보 글이 있을경우 입력합니다.
            if (addInfo != undefined) {
                xmlMsg += "ː";
                xmlMsg += addInfo;
            }

            xmlMsg.replace("\r\n", "<br>");

            var strUrl = m_WebRoot + "/Common/popup/Neoplus.ErrorMessage.aspx";

            if (document.getElementById("hidMenuID") != null) {
                strUrl += "?MenuID=" + document.getElementById("hidMenuID").value;
            }

            var strWidth = fn_GetWidth(397).toString();
            var strHeight = fn_GetHeight(193).toString();

            var strFeature = "status:no;dialogWidth:" + strWidth + "px;dialogHeight:" + strHeight + "px;help:no;scroll:no;";

            window.showModalDialog(strUrl, xmlMsg, strFeature);
        }

    }

    /************************************************************************
    함수명		: fn_Information
    작성목적	: 알림(안내) 메시지 상자를 띠운다.
    Parameter	: message - 코드 or 출력할 메시지
    Return		: alert으로 에러메세지 출력
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.Information = function fn_Information(message) {

        try {

            var xmlMsg;
            xmlMsg = fn_ParseXmlMessage(message);
            xmlMsg = "* [i]알림\r\n---------------------------------------------------\r\n " + xmlMsg
            alert(xmlMsg);

        }
        catch (exception) {
            alert("시스템 오류입니다. " + exception.description);
        }
    }

    /************************************************************************
    함수명		: NeoplusInformation
    작성목적	: 알림(안내) 메시지 상자를 띠운다.
    Parameter	: message - 코드 or 출력할 메시지
    addInfo - 추가 정보
    Return		: showModalDialog로 에러메세지 출력
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.NeoplusInformation = function fn_NeoplusInformation(message, addInfo) {

        fn_GetMessageType();

        if (m_MessageType == 'normal') {
            Common.Information(message);
        }
        else {

            var xmlMsg;
            xmlMsg = fn_ParseXmlMessage(message);
            // 추가정보 글이 있을경우 입력합니다.
            if (addInfo != undefined) {
                xmlMsg += "ː";
                xmlMsg += addInfo;
            }
            xmlMsg.replace("\r\n", "<br>");


            var strUrl = m_WebRoot + "/Common/popup/Neoplus.Information.htm";

            var strWidth = fn_GetWidth(397).toString();
            var strHeight = fn_GetHeight(193).toString();

            var strFeature = "status:no;dialogWidth:" + strWidth + "px;dialogHeight:" + strHeight + "px;help:no;scroll:no;";

            window.showModalDialog(strUrl, xmlMsg, strFeature);
        }
    }

    /************************************************************************
    함수명		: fn_Confirm
    작성목적	: 질문 상자를 띠운다. 사용자의 조작에 따라 true, false 를 반환한다.
    Parameter	: message - 코드 or 질문할 메시지
    Return		: true, false
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.Confirm = function fn_Confirm(message) {
        try {

            var xmlMsg;
            xmlMsg = fn_ParseXmlMessage(message);
            xmlMsg = "* [?]확인\r\n---------------------------------------------------\r\n " + xmlMsg
            res = confirm(xmlMsg);

            return res;
        }
        catch (exception) {
            alert("시스템 오류입니다. " + exception.description);
        }
    }

    /************************************************************************
    함수명		: fn_NeoplusConfirm
    작성목적	: 질문 상자를 띠운다. 사용자의 조작에 따라 true, false 를 반환한다.
    Parameter	: message - 코드 or 질문할 메시지
    Return		: true, false
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.NeoplusConfirm = function fn_NeoplusConfirm(message) {

        var winResult ;

        fn_GetMessageType();

        if (m_MessageType == 'normal') {
            winResult = Common.Confirm(message);
        }
        else {


            var xmlMsg;
            xmlMsg = fn_ParseXmlMessage(message);
            xmlMsg.replace("\r\n", "<br>");

            var strUrl = m_WebRoot + "/Common/popup/Neoplus.Confirm.htm";

            var strWidth = fn_GetWidth(397).toString();
            var strHeight = fn_GetHeight(193).toString();

            var strFeature = "status:no;dialogWidth:" + strWidth + "px;dialogHeight:" + strHeight + "px;help:no;scroll:no;";

            winResult = window.showModalDialog(strUrl, xmlMsg, strFeature);

        }

        return winResult;
    }

    /************************************************************************
    함수명		: fn_ModalPopup
    작성목적	: 모달 팝업창을 실행한다.
    Parameter	: 

    Return		: 모달창의 결과값 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.ModalOpen = function fn_ModalOpen(callUrl, argument, width, height) {

        var strReturn;

        var strWidth = fn_GetWidth(width).toString();
        var strHeight = fn_GetHeight(height).toString();

        var strUrl = m_WebRoot + "/Common/Popup/Neoplus.Modal.htm";
        var arrArgument = new Array(self, callUrl, argument);

        var strFeature = "status:no;dialogWidth:" + strWidth + "px;dialogHeight:" + strHeight + "px;help:no;scroll:no;resizable:no;center:yes";

        strReturn = window.showModalDialog(strUrl, arrArgument, strFeature);

        return strReturn;

    }


    /************************************************************************
    함수명		: fn_ModelessPopup ShowModalessDialog
    작성목적	: 모달리스 팝업창을 실행한다.
    Parameter	: 

    Return		: 모달리스창의 결과값 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.ModelessOpen = function fn_ModelessOpen(callUrl, argument, width, height) {

        var strReturn;

        var strWidth = fn_GetWidth(width).toString();
        var strHeight = fn_GetHeight(height).toString();

        var strUrl = m_WebRoot + "/Common/Popup/Neoplus.Modal.htm";
        var arrArgument = new Array(self, callUrl);

        var strFeature = "status:no;dialogWidth:" + strWidth + "px;dialogHeight:" + strHeight + "px;help:no;scroll:yes;resizable:no;center:yes ";

        strReturn = window.showModelessDialog(strUrl, arrArgument, strFeature);

        return strReturn;

    }

    /************************************************************************
    함수명		: fn_WindowOpen
    작성목적	: 일반 팝업창을 실행한다.
    Parameter	: 
    Return		: 일반창 오픈 후 결과값 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.WindowOpen = function fn_WindowOpen(url, formNm, width, height) {

        var wHeight = window.screen.availHeight;
        var wWidth = window.screen.availWidth;

        var strWidth = fn_GetWidth(width).toString();
        var strHeight = fn_GetHeight(height).toString();

        var ileft = 0;
        var itop = 0;

        ileft = parseInt((parseInt(wWidth) - parseInt(strWidth)) / 2);
        itop = parseInt((parseInt(wHeight) - parseInt(strHeight)) / 2);

        if (ileft < 0) ileft = 0;
        if (itop < 0) itop = 0;

        var strFeature = "toolbar=no,menubar=no,statusbar=no,scrollbars=no,resizable=no,";

        var form = window.open(url, formNm, strFeature + "height=" + strHeight + ",width=" + strWidth + ",top=" + itop + ",left = " + ileft);
        form.focus();

    }


    /************************************************************************
    함수명		: fn_WindowOpenScroll
    작성목적	: 스크롤이 가능한 일반 팝업창을 실행한다. 
    Parameter	: 
    Return		: 스크롤이 가능한 일반창 오픈 후 결과값 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.WindowOpenScroll = function fn_WindowOpenScroll(url, formNm, width, height) {

        var wHeight = window.screen.availHeight;
        var wWidth = window.screen.availWidth;

        var strWidth = fn_GetWidth(width).toString();
        var strHeight = fn_GetHeight(height).toString();

        var ileft = 0;
        var itop = 0;

        ileft = parseInt((parseInt(wWidth) - parseInt(strWidth)) / 2);
        itop = parseInt((parseInt(wHeight) - parseInt(strHeight)) / 2);

        if (ileft < 0) ileft = 0;
        if (itop < 0) itop = 0;

        var strFeature = "toolbar=no,menubar=no,statusbar=no,scrollbars=yes,resizable=no,";

        var form = window.open(url, formNm, strFeature + "height=" + strHeight + ",width=" + strWidth + ",top=" + itop + ",left = " + ileft);
        form.focus();

    }

    /************************************************************************
    함수명		: fn_WindowOpenResize
    작성목적	: 일반 팝업창을 실행한다. 스크롤과 리사이즈가 가능.
    Parameter	: 
    Return		: 일반창 오픈 후 결과값 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.WindowOpenResize = function fn_WindowOpenResize(url, formNm, width, height) {

        var wHeight = window.screen.availHeight;
        var wWidth = window.screen.availWidth;

        var strWidth = fn_GetWidth(width).toString();
        var strHeight = fn_GetHeight(height).toString();

        var ileft = 0;
        var itop = 0;

        ileft = parseInt((parseInt(wWidth) - parseInt(strWidth)) / 2);
        itop = parseInt((parseInt(wHeight) - parseInt(strHeight)) / 2);

        if (ileft < 0) ileft = 0;
        if (itop < 0) itop = 0;

        var strFeature = "toolbar=no,menubar=no,statusbar=no,scrollbars=yes,resizable=yes,";

        var form = window.open(url, formNm, strFeature + "height=" + strHeight + ",width=" + strWidth + ",top=" + itop + ",left = " + ileft);
        form.focus();

    }


    /************************************************************************
    함수명		: fn_DownFile
    작성목적	: 파일다운로드
    Parameter	: 
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:

    * 10MB 
    => 10485760바이트(B)
    => 10240킬로바이트(KB)
    *************************************************************************/
    this.DownFile = function fn_DownFile(key, fileLength) {

        var bIsDown = true;


        if (fileLength > 10485760) {
            bIsDown = confirm("파일의 크기가 10MB이상입니다. 다운로드 받는데 시간이 걸립니다. 다운로드 하시겠습니까? \r\n파일이 준비될동안 기다려 주십시오. ")
        }

        if (bIsDown) {
            var url = m_WebRoot + "/Common/FileDown.aspx?Key=" + key;

            document.getElementById("ifrFileDown").src = url;
        }
    }

    //    /************************************************************************
    //    함수명		: fn_WindowOpenScroll
    //    작성목적	: 스크롤이 가능한 일반 팝업창을 실행한다. 
    //    Parameter	: 
    //    Return		: 스크롤이 가능한 일반창 오픈 후 결과값 반환
    //    작 성 자	: (주)네오플러스 김윤식
    //    최초작성일	: 2010.03.12
    //    최종작성일	:
    //    수정내역	:
    //    *************************************************************************/
    //    this.SearchZipCode = function fn_SearchZipCode(zipCodeControlID, addressControlID) {

    //        var strReturn;

    //        var strWidth = fn_GetWidth(width).toString();
    //        var strHeight = fn_GetHeight(height).toString();

    //        var strUrl = m_WebRoot + "/Common/Popup/Neoplus.Modal.htm";
    //        var arrArgument = new Array(self, m_WebRoot + "/Common/popZipCode.aspx");

    //        var strFeature = "status:no;dialogWidth:" + strWidth + "px;dialogHeight:" + strHeight + "px;help:no;scroll:yes;resizable:no;center:yes";

    //        strReturn = window.showModalDialog(strUrl, arrArgument, strFeature);

    //        return strReturn;

    //    }


    /************************************************************************
    함수명		: fn_SearchZipCode
    작성목적	: 우편번호검색
    Parameter	: 우편번호컨트롤ID, 주소컨트롤ID
    Return		: Json객체
    Json객체 필드설명
    ZipCode : 우편번호
    , SiDo    : 시, 도                       
    , Gugun   : 군                 
    , Dong    : 동                     
    , Ri      : 리                     
    , Bunji   : 번지                     
    
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchZipCode = function fn_SearchZipCode(zipCodeID, addressID) {

        try {
            var width = 720;
            var height = 486;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/popZipcode.aspx";

            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            // 모달창에서 선택한 우편번호 정보 Json객체로 반환
            var oReturnJson = Common.ModalOpen(strUrl, "", width, height);


            // 값이 있을때만 바인딩 한다.
            if (oReturnJson != null) {
                document.getElementById(zipCodeID).value = oReturnJson.ZipCode;
                document.getElementById(addressID).value = oReturnJson.BasicAddress;
            }

            return oReturnJson;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }


    /************************************************************************
    함수명		: fn_SearchCode
    작성목적	: 코드검색
    Parameter	: 코드
    Return		: Json객체
    Json객체 필드설명
    CodeName                  -- 코드명 
    , Description               -- 설명 
    , MainCode                  -- 메인코드 
    , RowNo                     -- NO 
    , SortSEQ                   -- 정렬순서 
    , SubCode                   -- 서브코드 
    , UseYN                     -- 사용유무                 
    
    작 성 자	: (주)네오플러스 오진욱
    최초작성일	: 2010.06.24
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchCode = function fn_SearchCode(mainCode, code, name) {

        try {
            var width = 720;
            var height = 486;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/PopCodeSearch.aspx?MainCode=" + mainCode;
            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            // 모달창에서 선택한 사원 정보 Json객체로 반환
            var oReturnJson = Common.ModalOpen(strUrl, "", width, height)

            // 값이 있을때만 바인딩 한다.
            if (oReturnJson != null) {

                document.getElementById(code).value = oReturnJson.SubCode;
                document.getElementById(name).value = oReturnJson.CodeName;
            }

            return oReturnJson;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: fn_SearchCustomerCompany
    작성목적	: 거래처검색
    Parameter	: 거래처
    Return		: Json객체
    Json객체 필드설명

    txtcontrol 에는 거래처명
    hidcontrol 에는 거래처코드를 저장합니다.
    
    작 성 자	: (주)네오플러스 김훈
    최초작성일	: 2011.03.13
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchCustomerCompany = function fn_SearchCustomerCompany(txtControl, hidControl) {

        try {
            var width = 720;
            var height = 486;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/PopCustomerCompanySearch.aspx";
            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            // 모달창에서 선택한 사원 정보 Json객체로 반환
            var oReturnJson = Common.ModalOpen(strUrl, "", width, height)

            // 값이 있을때만 바인딩 한다.
            if (oReturnJson != null) {

                document.getElementById(txtControl).value = oReturnJson.CustomerName[0];
                document.getElementById(hidControl).value = oReturnJson.CustomerCode[0];
            }

            return oReturnJson;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: fn_SearchCustomerCompany
    작성목적	: 거래처 담당자 검색
    Parameter	: 거래처
    Return		: Json객체
    Json객체 필드설명

    txtcontrol 에는 거래처담당자명
    hidcontrol 에는 거래처담당자코드를 저장합니다.
    CustomerCode 는 거래처업체의 해당 코드로 선 조회

    작 성 자	: (주)네오플러스 김훈
    최초작성일	: 2011.03.13
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchCustomerCharge = function fn_SearchCustomerCompany(txtControl, hidControl, CustomerCode) {
        try {
            var width = 720;
            var height = 486;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/PopCustomerChargeSearch.aspx?Code=" + CustomerCode;
            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            // 모달창에서 선택한 사원 정보 Json객체로 반환
            var oReturnJson = Common.ModalOpen(strUrl, "", width, height)

            // 값이 있을때만 바인딩 한다.
            if (oReturnJson != null) {

                document.getElementById(txtControl).value = oReturnJson.ChargeName[0];
                document.getElementById(hidControl).value = oReturnJson.ChargeCode[0];
            }

            return oReturnJson;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: fn_SearchSysUser
    작성목적	: 직원  검색후 컨트롤에 저장
    Parameter	: 직원검색
    Return		: Json객체
    Json객체 필드설명

    txtcontrol 에는 직원명
    hidcontrol 에는 직원코드를 저장합니다.
    
    작 성 자	: (주)네오플러스 김훈
    최초작성일	: 2011.03.13
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchSysUser = function fn_SearchSysUser(txtControl, hidControl) {

        try {
            var width = 720;
            var height = 486;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/PopSysUserSearch.aspx";
            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            // 모달창에서 선택한 사원 정보 Json객체로 반환
            var oReturnJson = Common.ModalOpen(strUrl, "", width, height)

            // 값이 있을때만 바인딩 한다.
            if (oReturnJson != null) {

                document.getElementById(txtControl).value = oReturnJson.UserName[0];
                document.getElementById(hidControl).value = oReturnJson.UserID[0];
            }

            return oReturnJson;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }



    /************************************************************************
    함수명		: fn_SearchSearchCustomer
    작성목적	: 거래처검색
    Parameter	: 거래처
    Return		: Json객체
    Json객체 필드설명
    Name                  -- 거래처명
    , Phone               -- 번호
    , Fax                 -- 팩스
    , Address             -- 주소
    , BusinessContacts    -- 대표이름 
    , RowNo               -- NO            
    
    작 성 자	: (주)네오플러스 진수
    최초작성일	: 2011.02.17
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchCustomer = function fn_SearchSearchCustomer(CustomerSEQ, Name) {

        try {
            var width = 720;
            var height = 486;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/PopCustomersSearch.aspx";
            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            // 모달창에서 선택한 사원 정보 Json객체로 반환
            var oReturnJson = Common.ModalOpen(strUrl, "", width, height)

            // 값이 있을때만 바인딩 한다.
            if (oReturnJson != null) {

                document.getElementById(CustomerSEQ).value = oReturnJson.CustomerSEQ.toString();
                document.getElementById(Name).value = oReturnJson.Name.toString();
            }

            return oReturnJson;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: fn_SearchDept
    작성목적	: 조직도 검색
    Parameter	: 이름컨트롤ID, 코드HiddenID, 부서선택보드(U:사용자, D:부서, MU:멀티사용자, MD:멀티부서, UD:다중사용자부서)
    Return		: array

    작 성 자	: (주)네오플러스 오진욱
    최초작성일	: 2010.06.24
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchDept = function fn_SearchDept(hhdCode, txtName, mode) {

        try {
            var width = 920;
            var height = 686;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/PopDeptSearch.aspx?Mode=" + mode;
            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            var oReturn;
            var args = new Array();
            args["code"] = document.getElementById(hhdCode).value;

            switch (mode) {
                case "U": //사용자
                    oReturn = Common.ModalOpen(strUrl, args, fn_GetWidth(455), fn_GetHeight(726));
                    break;
                case "D": //부서
                    oReturn = Common.ModalOpen(strUrl, args, fn_GetWidth(455), fn_GetHeight(726));
                    break;
                case "MU": //멀티사용자
                    oReturn = Common.ModalOpen(strUrl, args, fn_GetWidth(920), fn_GetHeight(726));
                    break;
                case "MD": //멀티부서
                    oReturn = Common.ModalOpen(strUrl, args, fn_GetWidth(920), fn_GetHeight(726));
                    break;
                case "UD": //다중사용자부서
                    oReturn = Common.ModalOpen(strUrl, args, fn_GetWidth(920), fn_GetHeight(726));
                    break;
                default: break;
            }

            // 값이 있을때만 바인딩 한다.
            if (oReturn != null) {
                document.getElementById(hhdCode).value = oReturn.split('?')[0];
                document.getElementById(txtName).value = oReturn.split('?')[1];
            }

            return oReturn;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: fn_SearchMenu
    작성목적	: 메뉴검색
    Parameter	: 
    Return		: Array
    Array 필드설명
    MenuID        -- 메뉴ID
    , MenuName      -- 메뉴명
    , MenuFullName  -- 메인명전체

    
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.08.03
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SearchMenu = function fn_SearchMenu() {

        try {
            var width = 675;
            var height = 486;
            var strRetVal;

            // 경로수정
            var strUrl = m_WebRoot + "/Common/PopMenuSearch.aspx";
            var strWidth = fn_GetWidth(width).toString();
            var strHeight = fn_GetHeight(height).toString();

            // 모달창에서 선택한 사원 정보 Json객체로 반환
            var oReturnArray = Common.ModalOpen(strUrl, "", width, height)

            return oReturnArray;
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }

    }


    //** 현재 선택된 다국어 입력 inputID
    this.currentID = '';
    this.hhdValueID = '';

    /************************************************************************
    함수명		: fn_MultiLanguageView
    작성목적	: 언어별 텍스트 보기 창 열기
    Parameter	: 현재 선택된 언어로 보여지는 텍스트, 모든 텍스트
    Return		: 없음          
    
    작 성 자	: (주)네오플러스 오진욱
    최초작성일	: 2010.06.30
    최종작성일	:
    수정내역	: 
    *************************************************************************/
    this.MultiLanguageView = function fn_MultiLanguageView(currentID, hhdValueID) {

        try {
            this.currentID = currentID;
            this.hhdValueID = hhdValueID;

            if ($("#" + hhdValueID).val().length > 0) {

                var oJson = eval("(" + $("#" + hhdValueID).val().toString().replace(/＇/gi, "'") + ")");

                $("#dvLanguageView_ko-KR").val(oJson["ko-KR"]);
                $("#dvLanguageView_en-US").val(oJson["en-US"]);
                $("#dvLanguageView_ja-JP").val(oJson["ja-JP"]);
                $("#dvLanguageView_zh-CN").val(oJson["zh-CN"]);

                $.blockUI({ message: $("#dvLanguageView"), css: { borderColor: '#efefef', display: '', cursor: 'default', width: '300px'} });
            }
            else {
                return false;
            }
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: MultiLanguageInput
    작성목적	: 언어별 텍스트 입력 창 열기
    Parameter	: 현재 선택된 언어로 보여지는 텍스트, 모든 텍스트
    Return		: 없음          
    
    작 성 자	: (주)네오플러스 오진욱
    최초작성일	: 2010.06.30
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.MultiLanguageInput = function fn_MultiLanguageInput(currentID, hhdValueID) {

        try {
            this.currentID = currentID;
            this.hhdValueID = hhdValueID;

            if ($("#" + hhdValueID).val().length > 0) {

                var oJson = eval("(" + $("#" + hhdValueID).val().toString().replace(/＇/gi, "'") + ")");

                //alert(document.getElementById("hidCurtureID").value);
                var strNowCurture = document.getElementById("hidCurtureID").value;

                //alert($("#" + currentID).val());
                oJson[strNowCurture] = $("#" + currentID).val();

                $("#dvLanguageInput_ko-KR").val(oJson["ko-KR"]);
                $("#dvLanguageInput_en-US").val(oJson["en-US"]);
                $("#dvLanguageInput_ja-JP").val(oJson["ja-JP"]);
                $("#dvLanguageInput_zh-CN").val(oJson["zh-CN"]);

                $.blockUI({ message: $("#dvLanguageInput"), css: { borderColor: '#efefef', display: '', cursor: 'default', width: '300px'} });

            }
            else {
                return false;
            }
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }

    }


    /************************************************************************
    함수명		: MultiLanguageSave
    작성목적	: 언어별 텍스트 입력
    Parameter	: 없음
    Return		: 없음          
    
    작 성 자	: (주)네오플러스 오진욱
    최초작성일	: 2010.06.30
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.MultiLanguageSave = function fn_MultiLanguageSave() {
        try {
            //저장 작업(Json 만들기)

            //alert(document.getElementById("hidCurtureID").value);
            var strNowCurture = document.getElementById("hidCurtureID").value;

            //alert($("#dvLanguageInput_" + strNowCurture).val());
            document.getElementById(this.currentID).value = $("#dvLanguageInput_" + strNowCurture).val();

            var strCurtureString = "{" + "＇ko-KR＇:＇" + $("#dvLanguageInput_ko-KR").val() + "＇, ";
            strCurtureString += "＇en-US＇:＇" + $("#dvLanguageInput_en-US").val() + "＇, ";
            strCurtureString += "＇ja-JP＇:＇" + $("#dvLanguageInput_ja-JP").val() + "＇, ";
            strCurtureString += "＇zh-CN＇:＇" + $("#dvLanguageInput_zh-CN").val() + "＇ }";

            //alert(strCurtureString);
            document.getElementById(this.hhdValueID).value = strCurtureString;

            $.unblockUI();
            this.currentID = '';
            this.hhdValueID = '';
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: MultiLanguagePopupClose
    작성목적	: 팝업 닫기(닫은 후 처리가 필요해서)
    Parameter	: 없음
    Return		: 없음          
    
    작 성 자	: (주)네오플러스 오진욱
    최초작성일	: 2010.06.30
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.MultiLanguagePopupClose = function fn_MultiLanguagePopupClose() {
        try {
            $.unblockUI();
            this.currentID = '';
            this.hhdValueID = '';
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }

    /************************************************************************
    함수명		: SendMessage
    작성목적	: 메시지 보내기
    Parameter	: 없음
    Return		: 없음          
    
    작 성 자	: (주)네오플러스 오진욱
    최초작성일	: 2010.06.30
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SendMessage = function SendMessage(id) {
        try {
            fn_WindowOpen('/Main/Common/PopMessageSend.aspx?id=' + id, 'PopMessageSend', 920, 685);
        }
        catch (e) {
            Common.NeoplusErrorMessage(e.toString());
        }
    }
}

// 스크립트 선언
var Common = new Common();


String.format = function (text) {
    if (arguments.length <= 1) {
        return text;
    }
    var tokenCount = arguments.length - 2;
    for (var token = 0; token <= tokenCount; token++) {
        text = text.replace(new RegExp("\\{" + token + "\\}", "gi"), arguments[token + 1]);
    }
    return text;
};
