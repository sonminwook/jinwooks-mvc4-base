function NeoplusWebControl() {

    /************************************************************************
    함수명		: f_onfocus -외부노출함수
    작성목적	: 각 element의 focus를 위한 이벤트핸들러 
    Parameter 	: 
    Return		: event
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/    
    var _onfocus = 0;
    this.OnFocus = function f_onfocus() {
        var e = e || window.event;
        var ctrl = e.target || e.srcElement;

        if (ctrl.select) ctrl.select(); // 선택
        _onfocus = 1;
    }

    /************************************************************************
    함수명		: f_onblur() -외부노출함수
    작성목적	: 각 element의 blur를 위한 이벤트핸들러 
    Parameter 	: 
    Return		: event
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/        
    this.OnBlur = function f_onblur(valid) {
        var e = e || window.event;
        var activeElement = e.target || e.srcElement;

        if (activeElement.value != '' && valid != '') {
            activeElement.value = FormatValid(activeElement.value, valid); //FormatValid(RoundFloat(event.srcElement.value, valid), valid);
        }
    }
    
    /************************************************************************
    함수명		: FormatValid() -내부처리함수
    작성목적	: 소수점 이하자릿수 체크 하기
    Parameter 	: 
    Return		: event
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/            
    function FormatValid(value, valid) {
    value = value.toString();

        // 소수점 이하의 수가 원래 유효숫자보다 적을 경우 0 을 채워준다.						
        if (valid != '') // 유효자리가 지정되어있을 경우
        {
            var valArray = value.split(".");
            var isFloat = (value.indexOf(".") != -1);
            var figureLength = 0;
            if (valArray.length > 1) {
                figureLength = valArray[1].length;
            }

            //alert(figureLength);

            if (figureLength == parseInt(valid)) {
                return value;
            }

            if (figureLength > parseInt(valid)) {
                return valArray[0] + '.' + valArray[1].substring(0, valid);
            }
        }
        return value;
    }

    /************************************************************************
    함수명		: fn_CommaNum -내부처리함수
    작성목적	: 화폐형으로 문자열을 리턴한다.
                  음수도 처리가 가능함
    Parameter 	: 문자열
    Return		: 화폐형 문자열
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/ 
    function fn_CommaNum( num )
    {  
        var bIsMinus = false ;

        if (num.substr(0,1) == "-")
        {
            bIsMinus = true;
            num = num.replace("-", "");
        }
           
        try
        {
        
	        num = new String(num);
	        var sign='';
	        var temp='';
	        var pos=3;
    	    
	        num_len = num.length;

	        while (num_len>0) 
	        {	
		        num_len=num_len-pos;
		        if(num_len<0) 
		        {		
			        pos=num_len+pos;
			        num_len=0;	
		        }
        			
		        temp=','+num.substr(num_len,pos)+temp;
	        }
        	
	        strResult = sign+temp.substr(1);
        	
	        if (bIsMinus)
	        {
	            strResult2 = "-" + strResult ;
	        }
	        else
	        {
	            strResult2 = strResult ;
	        }
    	    
        	
	        return strResult2;
    	
	    }
	    catch(e)
	    {
	        alert(e);
	    }
	    finally
	    {
	    }
    }

    /************************************************************************
    함수명		: fn_CheckCurrency -외부노출함수
    작성목적	: 입력시 화폐형 문자열인지 체크하고 화폐형에 맞추어 문자열 반환
    Parameter 	: 컨트롤
    Return		: 화폐형 문자열
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/ 
    this.CheckCurrency = function fn_CheckCurrency( Control )
    {

        exp_str = "-?\\d{1,}([.]\\d{1,4})*";

        if (Control.value != "-")
        {

            var tmp = Control.value.split(',');
            objValue = tmp.join("");                        
            objValue = objValue.replace("-", "");

            if (!fn_CheckMatch(objValue, exp_str))
            {
	            Common.NeoplusErrorMessage("숫자로만 입력하여 주십시오.") ;
                var iLength = Control.value.length -1 ;
		        Control.value = Control.value.substr(0, iLength) ;
		        Control.focus();
		        return;
            }
               	
	        var str = fn_CommaNum( Control.value.replace(/,/g, ''));
	        Control.value = str;
	    }
    }
    // ---- 화폐형 체크 끝


    /************************************************************************
    함수명		: fn_CheckNumber -외부노출함수
    작성목적	: 입력시 숫자형인지 체크한다.
    Parameter 	: 컨트롤
    Return		: 화폐형 문자열
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/     
    this.CheckNumber = function fn_CheckNumber( control )
    {

        if ( isNaN(control.value.replace(/,/g, '')) )
	    {
	        //fn_ErrorMessage("숫자로만 입력하여 주십시오.") ;
	        Common.NeoplusErrorMessage("숫자로만 입력하여 주십시오.") ;

		    control.value = control.value.substr(0,control.value.length -1);
		    control.focus();
		    return;
	    }
    }
    // --- 단순 숫자형 체크 끝




    // -- 입력가능 문자열 길이 체크 시작
    /************************************************************************
    함수명		: fn_GetByteLength -내부사용함수
    작성목적	: 입력컨트롤내의 문자열 Byte길이를 반환한다.
    Parameter 	: 컨트롤
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/  
    function fn_GetByteLength(control) {

	    var byteLength = 0;
	    for (var inx = 0; inx < control.value.length; inx++) {
		    var oneChar = escape(control.value.charAt(inx));
		    if ( oneChar.length == 1 ) {
			    byteLength ++;
		    } else if (oneChar.indexOf('%u') != -1) {
			    byteLength += 2;
		    } else if (oneChar.indexOf('%') != -1) {
			    byteLength += oneChar.length/3;
		    }
	    }
	    
	    return byteLength;
    }


    /************************************************************************
    함수명		: fn_GetString -내부사용함수
    작성목적	: 입력컨트롤내의 문자열을 길이만큼만 허용하고 나머지는 삭제하고 반환다.
    Parameter 	: 컨트롤, 길이
    Return		: 컨트롤 문자열중 최대 길이 이하가 삭제된 문자열
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/  
    function fn_GetString(control, len)
    {
	    var byteLength = 0;
	    var result = '';
	    var type = 0;

	    for (var inx = 0; inx < control.value.length; inx++) {
		    var oneChar = escape(control.value.charAt(inx));
		    if ( oneChar.length == 1 ) {
			    byteLength ++;
			    type = 0;
		    } else if (oneChar.indexOf('%u') != -1) {
			    byteLength += 2;
			    type = 1;
		    } else if (oneChar.indexOf('%') != -1) {
			    byteLength += oneChar.length/3;
			    if (  oneChar.length/3 > 1 )
				    type = 1;
			    else
				    type = 0;
		    }
    		
		    if ( type == 1)
		    {
			    result = result + control.value.charAt(inx);
			    if ( byteLength > len )
				    return result.substring(0,inx);
		    }
		    else
		    {
			    if ( byteLength > len )
				    return result.substring(0,inx+1);
			    result = result + control.value.charAt(inx);   			
		    }
	    }
	    
	    return result;
    }

    /************************************************************************
    함수명		: fn_CheckMaxLength - 외부노출함수
    작성목적	: 컨트롤의 최대 길이를 체크하고 최대길이 이상이 되면 삭제하고 반환한다.
    Parameter 	: 컨트롤
    Return		: 최대길이가 체크된 문자열
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/ 
    this.CheckMaxLength = function fn_CheckMaxLength( control )
    {
	    var maxlen = 0;

	    if( control.isMultiLine )
		    maxlen = control.getAttribute("MultiLineMaxLength")
	    else
		    maxlen = control.getAttribute("maxLength")

	    if ( fn_GetByteLength(control) > maxlen ) 
	    {
		    control.value = fn_GetString(control, maxlen);
		    Common.NeoplusErrorMessage("최대입력가능 길이는 한글 " + Math.floor(maxlen/2) + "글자, 영문 " + maxlen + "글자 입니다.");
		    return;
	    } 
    }
   
    /************************************************************************
    함수명		: fn_EnterSubmit - 외부노출함수
    작성목적	: 엔터키시에 컨트롤 이동하기
    Parameter 	: 
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/         
    this.EnterSubmit = function fn_EnterSubmit(control) {

        if (event.keyCode == 13) {

    	    var ctl = document.getElementById(control.getAttribute("SubmitButton"));

            if (ctl == null) {
                return false;
            }
            else {
                ctl.click();
                return false;
            }
        }
        else{
            return true;
        }
    }   
   
  
    /************************************************************************
    함수명		: fn_EnterOrder - 외부노출함수
    작성목적	: 엔터키시에 컨트롤 이동하기
    Parameter 	: 
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/     
    this.EnterOrder = function fn_EnterOrder(control)
    {

	    try
        {
            // 엔터키이면 아래 실행..
            if (window.event.keyCode == 13 )
            {           
                // 페이지 베이스에서 각 컨트롤들을 변수에 담고 있다. 담겨 있는 변수의 문자열을 빼온다.
                var oNextControl = document.getElementById(control.getAttribute("NextControl"));

                if (oNextControl == null) {
                    return true;
                }
                else {
                    oNextControl.focus();
                    return false ;
                }
            }
            else
            {
		        return true ;
		    }
        }
        catch(e)
        {
            control.focus();
            return false ;
        }
        finally
        {}
    }

    /************************************************************************
    함수명		: fn_CheckDate - 외부노출함수
    작성목적	: 날짜 유효성체크
    Parameter 	: 컨트롤
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/     
    this.CheckDate = function fn_CheckDate(control) {

       // 컨트롤에 날짜 형식을 입력한다.
       var str = control.value;

       if (str != "") 
       {
		    if(str.match(/[0-9/-]+/)!=str)
		    {
			    Common.NeoplusErrorMessage("날짜에는 숫자만 됩니다 !");
			    //Control.value = "";
			    return false;
		    }
		    else {
    		
			    var che_year;
		 	    var che_month;
		 	    var che_day;

          	    if(str.length == 10) {
              	
  		 		    che_year  = str.substr(0,4)
  		 		    che_month = str.substr(5,2)
  		 		    che_day   = str.substr(8,2)
  		 		    che_chk1  = str.substr(4,1)
  		 		    che_chk2  = str.substr(7,1)

  		            if(che_chk1!="-" || che_chk2 !="-") {  
          		    
  		              Common.NeoplusErrorMessage ("유효한 날짜형식이 아닙니다.");
                      control.focus();
                      return false;
                    }

                    if ( fn_DateCheck(che_year,che_month,che_day ) == false) {
                      Common.NeoplusErrorMessage ("유효한 날짜가 아닙니다.");
                      control.focus();
                      return false;
                    }

    		    } 
    		    else {
        		
			        Common.NeoplusErrorMessage("10자리의 날짜로 입력 하세요.");
			        control.focus();
			        return false;
    	    	
	    	    }
            }
	    }
    }

    /************************************************************************
    함수명		: fn_DateCheck - 내부사용함수
    작성목적	: 유효일 날짜인지 체크
    Parameter 	: 년,월,일
    Return		: True, False로 반환
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/     
    function fn_DateCheck(v_year,v_month,v_day )
    {
	    var err = 0
	    if ( v_year.length != 4) err = 1
	    if ( v_month.length != 1 &&  v_month.length !=  2 ) err = 1
	    if ( v_day.length != 1  &&  v_day.length !=  2) err = 1

	    r_year = eval(v_year) ;
	    r_month = eval(v_month);
	    r_day = eval(v_day)  ;

	    if (r_month<1 || r_month>12) err = 1
	    if (r_day<1 || r_day>31) err = 1
	    if (r_year<0 ) err = 1

	    if (r_month==4 || r_month==6 || r_month==9 || r_month==11)
	    {
		    if (r_day==31) err=1
	    }

	    // 2,윤년체크
	    if (r_month==2){
		    var g=parseInt(r_year/4)

		    if (isNaN(g))
		    {
			    err=1
		    }
		    if (r_day>29) err=1
		    if (r_day==29 && ((r_year/4)!=parseInt(r_year/4))) err=1
	    }

	    if (err==1)
	    {
		    return false
	    }
	    else
	    {
	        return true;
	    }
    }

    /************************************************************************
    함수명		: fn_CheckDateStr - 외부노출함수
    작성목적	: 날짜형식이 맞는지 체크한다.
    Parameter 	: 컨트롤
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/ 
    this.CheckDateStr = function fn_CheckDateStr(control)
    {
	    try
	    {  		
		    var strDate = control.value ;
		    if(strDate.match(/[0-9/-]+/)!=strDate)
		    {		
			    var iLength = strDate.length -1 ;
		        control.value = strDate.substr(0, iLength) ;
		        control.focus();
  			
			    return false;
		    }
	    }
	    catch(e)
	    {
		    return false;
	    }
    }

    // 
    /************************************************************************
    함수명		: fn_DateDiff - 내부사용함수
    작성목적	: 두날짜기 기간 비교. 두날짜의 크기를 비교하여 True,False로 반환한다.
    Parameter 	: 첫번째일자컨트롤, 두번째일자컨트롤, 에러메세지
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/     
    function fn_DateDiff(fdate, bdate, strmsg)
    {
	    var str;
	    var str1;
	    str = eval(fdate+".value");
	    str1 = eval(bdate+".value");

	    var fdatev = str.substr(0,4)+str.substr(5,2)+str.substr(8,2);
	    var bdatev = str1.substr(0,4)+str1.substr(5,2)+str1.substr(8,2);
    	
	    if (fdatev > bdatev)
	    {
		    alert(strmsg);
		    return false;
	    }
	    else
	    {
		    return true;
	    }
    }

    // 
    /************************************************************************
    함수명		: fn_CheckMatch - 내부사용함수
    작성목적	: 항목검사후 가능한지 여부 체크
    Parameter 	: 문자열, 문자열을 검사할 정규식
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/     
    function fn_CheckMatch(objValue, exp_str) 
    {

        var r = new RegExp(exp_str, "g");

        var matches = r.exec(objValue);

        if (matches == null || objValue != matches[0]) 
        {       
            return false ;
        }
        
        return true;
    }


    // 컨트롤 유효성 검사
    function fn_Validate(ctls)
    {
    
        var strCtrlNms = "";

        var bReturnCheck = true ;

        for (var i = 0; i < ctls.childNodes.length; i++) {
        
            // 컨테이너 컨트롤의 값을 잡아온다.
            var obj = ctls.childNodes.item(i);

            if ( obj.hasChildNodes)
            {
                bReturnCheck = fn_Validate(obj) ;

                if (!bReturnCheck)
                {
                    return false ;
                }
            }

            //if ( formObj(i).id != "" && formObj(i).id != null && formObj(i).Group != null)
            if (obj.id != "" && obj.id != null) 
            {

                if (true) //document.activeElement.id != null && IsGroup(obj)
                {
                    // 1. 필수 입력 항목체크
                    if ( (obj.getAttribute("Required") == "True" || obj.getAttribute("Essential") == "True") && obj.value == "" )
                    {
                        // obj.Description.toString() ;
                        
                        Common.NeoplusErrorMessage(obj.getAttribute("Description") + "은(는) 필수입력 항목입니다. ");
                        obj.focus();
                        bReturnCheck = false;
                        return false ;
                    }
                    
                    /* 			
                    Currency = 1,	 // 화폐
                    Number = 2,		 // 숫자
                    --Date = 3,        // 날짜형
                    EMail = 4,       // 이메일  
                    PhoneNo = 5,     // 전화번호 
                    PostNo = 6,      // 우편번호
                    SocialNo = 7,    // 주민번호
                    BusinessNo = 8   // 사업자번호 */
                               
                    switch ( obj.getAttribute("ValidationType") )
                    {

                        // 숫자, 화폐형식 체크 
                        case "Currency" :
                        case "Numeric" :
                       
                            exp_str = "-?\\d{1,}([.]\\d{1,4})*";
                            
                            if (obj.value!= '')
                            {
                                var tmp = obj.value.split(',');
                                objValue = tmp.join("");                        

                                if (!fn_CheckMatch(objValue, exp_str))
                                {
                                    Common.NeoplusErrorMessage(obj.getAttribute("Description") + "은(는) 숫자로만 입력가능합니다.");
                                    obj.focus();
                                    bReturnCheck = false;
                                    return false ;
                                }
                            }
                        
                        break;
                    
                    
                        case "EMail" :
                        
                            exp_str = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

                            if (!fn_CheckMatch(obj.value, exp_str))
                            {
                                Common.NeoplusErrorMessage(obj.getAttribute("Description") + "의 이메일 형식이 틀립니다.\r\n 예)sample@kicm.kr");
                                obj.focus();
                                bReturnCheck = false;
                                return false ;
                            }
                        
                        break; 
                        
                        
                        case "PhoneNo" :
                        
                            exp_str = "0\\d{1,3}[-)]\\d{3,4}[-]\\d{4}";

                            if (!fn_CheckMatch(obj.value, exp_str))
                            {
                                Common.NeoplusErrorMessage(obj.getAttribute("Description") + "의 전화번호 형식이 틀립니다.\r\n 예)012-123-1234 또는 052)123-1234");
                                obj.focus();
                                bReturnCheck = false;
                                return false ;
                            }
                        
                        break;                     
                        
                        
                        case "PostNo" :
                        
                            exp_str = "\\d{3}-\\d{3}";

                            if (!fn_CheckMatch(obj.value, exp_str))
                            {
                                Common.NeoplusErrorMessage(obj.getAttribute("Description") + "의 우편번호 형식이 틀립니다.\r\n 예)123-123");
                                obj.focus();
                                bReturnCheck = false;
                                return false ;
                            }
                        
                        break; 
                        
                        case "SocialNo" :
                        
                            exp_str = "\\d{6}" + "-" + "?\\d{7}";

                            if (!fn_CheckMatch(obj.value, exp_str))
                            {
                                Common.NeoplusErrorMessage(obj.getAttribute("Description") + "의 주민등록번호 형식이 틀립니다.");
                                obj.focus();
                                bReturnCheck = false;
                                return false ;
                            }
                        
                        break;       
                        
                        // 사업자번호
                        case "BusinessNo" :
                        
                            exp_str = "\\d{3}" + "-" + "\\d{2}" + "-" + "\\d{5}";

                            if (!fn_CheckMatch(obj.value, exp_str))
                            {
                                Common.NeoplusErrorMessage(obj.getAttribute("Description") + "의 사업자번호 형식이 틀립니다.\r\n 예)123-12" + "-" + "12345");
                                obj.focus();
                                bReturnCheck = false;
                                return false ;
                            }
                        
                        break;    
                        
                        //Alpabetic Type Check 
                        case "Letter":
                           
                            exp_str = "[a-zA-Z]+";
                           
                            if (!fn_CheckMatch(obj.value, exp_str))
                            {
                                Common.NeoplusErrorMessage(obj.getAttribute("Description") + "은(는) 영문자(Alpabet)만 입력가능합니다.");
                                obj.focus();
                                bReturnCheck = false;
                                return false ;
                            }                           
                           
                         break; 
                        
                                                                         
                   
                    } // end switch
                    
                    
                    // 텍스트 입력이면 고정문자와, 최소입력을 체크한다.
                    /*----------------------------------------------------------------------*/
                    if (obj.type == "text") 
                    {

                        if (obj.value != "") 
                        {

                            //FixLength Check		
                            if (obj.getAttribute("FixLength") != null && obj.getAttribute("FixLength") != "0") {

                                if (obj.value.length != obj.getAttribute("FixLength")) {
                                    
                                    Common.NeoplusErrorMessage(obj.getAttribute("Description") + "은(는) 반드시 " + obj.getAttribute("FixLength") + "자리 이어야 합니다.");
                                    obj.focus();
                                    bReturnCheck = false;
                                    return false ;
                                }
                            }
                            else {

                                //MinLength Check		
                                if (obj.getAttribute("MinLength") != null && obj.getAttribute("MinLength") != "0") 
                                {
                                    if (obj.value.length < obj.getAttribute("MinLength")) 
                                    {
                                        Common.NeoplusErrorMessage(obj.getAttribute("Description") +  "은(는) 반드시 " + obj.getAttribute("MinLength") + "자리 이상이어야 합니다.");
                                        obj.focus();
                                        bReturnCheck = false;
                                        return false ;
                                    }
                                }
                            }
                            
                        } //if 
                        
                    }
                    /*----------------------------------------------------------------------*/

                }
            }

        }

        return bReturnCheck ;
    }


    // 
    /************************************************************************
    함수명		: fn_ClientValidate
    작성목적	: Table, Div등 컨테이너 컨트롤에 포함된 컨트롤을 모두 유효성에 적합한지 검사한다.
                  Noeplus.WebControl에 있는 Attribute를 참고하여 유효성 검사를 한다.
                  
                  사용하는 Attribute 아래와 같다.
                  Required : 필수항목
                  Description : 컨트롤 설명
                  ContolType : 컨트롤의 입력형식
                                
                                   
    Parameter 	: Table, Div등이 컨테이너 컨트롤
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/     
    this.ClientValidate = function fn_ClientValidate(ctlID) {

        var DATE_SEPARATOR = '-'; //날짜구분자에 대한 상수정의

        var bCheckSave = false ;

        if (ctlID == undefined) {
            Common.NeoplusInformation("유효성 검사를 할 컨트롤을 지정하세요");
            bCheckSave = false;
        }
        else {

            var ctl = document.getElementById(ctlID);

            if (ctl == undefined) {
                Common.NeoplusInformation(ctlID + " 컨테이너 컨트롤을 찾을수 없습니다.");
                bCheckSave = false;
            }
            else {


                bCheckSave = fn_Validate(ctl) ;
            }
        }

        return bCheckSave ;

    }
    
    /************************************************************************
    함수명		: fn_ValidateListControl(ctlID)
    작성목적	: 리스트 컨트롤의 필수 입력 여부를 확인합니다.
                  DropDownList, RadioButtonList
                  
    Parameter	:
    Return		:
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.ValidateListControl = function fn_ValidateListControl(ctlID) {

        var bCheck = true;

        try {
        
            // 라디오버튼 컨트롤을 찾는당...
            var oListControl = $("[id=" + ctlID + "]");
           
            // SourceField가 있는지 확인
            if ( (oListControl.attr("Required") != undefined &&  oListControl.attr("Required") != null)
            || (oListControl.attr("Essential") != undefined &&  oListControl.attr("Essential") != null)) {
              
                // 소스필드가 있으면 소스필드에 해당하는 값을 가져온다.
                if ( oListControl.attr("Required") == "True" || oListControl.attr("Essential") == "True")
                {
                
                    bCheck = false;
                
                    var oListItem = $("input[id^=" + ctlID + "]");

                    for (var i = 0; i < oListItem.length; i++) {

                        if (oListItem[i].checked) {
                        
                            bCheck = true;
                            break;
                        }
                    }                    
                }
            }                
            
            if (!bCheck)
            {
                Common.NeoplusErrorMessage(oListControl.attr("Description") + "은(는) 필수 입력 항목입니다.");
                oListControl.focus();
            }
            
            
            return bCheck ;
                
        }
        catch (exception) {
        }       
    }    
    
    
    
    

    function fn_ResetInputSub( ctls )
    {
    
        // 컨트롤수만큼 
        for (var i = 0; i < ctls.childNodes.length; i++) {
        
            // 컨테이너 컨트롤의 값을 잡아온다.
            var objSub = ctls.childNodes.item(i);

            if ( objSub.hasChildNodes)
            {
                fn_ResetInputSub(objSub) ;
            }

            //strTest = strTest +  'ID:' + ctls[i].id +  ', tagName:' + ctls[i].tagName + ', type:' + ctls[i].type + '<br>';
            
            // TextBox, CheckBox, CheckBoxList, RadioButtonList, hidden에 해당하는 컨트롤의 값을 초기화 한다.
            if (objSub.tagName == "INPUT" || objSub.tagName == "TEXTAREA")
            {
                objSub.readOnly = false;
                objSub.disabled = false;
            
                // hidden필드, TextBox, 멀티입력TextBox
                if (objSub.type == "hidden" || objSub.type == "text" || objSub.type == "textarea" ) 
                {
                    // ABS가 True가 아니면 값초기화
                    if (objSub.getAttribute("ABS") == null || objSub.getAttribute("ABS") == "False") 
                    {
                        objSub.value='';
                        
                        // 입력형식이 Currency이면 기본값으로 0을 대입하여 준다.
                        if (objSub.getAttribute("ValidationType") == "Currency") 
                        {
                            objSub.value = '0';
                        }                        
                    }
                }      
                
                // CheckBox CheckBoxList
                if (objSub.type == "checkbox")
                {
                    // ABS가 True가 아니면 값초기화
                    if (objSub.getAttribute("ABS") == null || objSub.getAttribute("ABS") == "False") 
                    {
                        objSub.checked = false; 
                    }                                   
                } 
                
                // 라이오버튼 또는 라이오버튼 리스트
                if (objSub.type == "radio")
                {
                    // ABS가 True가 아니면 값초기화
                    if (objSub.getAttribute("ABS") == null || objSub.getAttribute("ABS") == "False") 
                    {
                        objSub.checked = false;
                    }                                   
                }
                
                
            }
                      
            // DropDownList, ListBox
            if (objSub.tagName == "SELECT")
            {
                objSub.readOnly = false;
                objSub.disabled = false;

                // 단일선택
                if (objSub.type == "select-one") //
                {
                    //objSub.className = "select";				
                
                    //상태, 값을 유지할 컨트롤이 아니면 기본값으로 설정
                    if (objSub.getAttribute("ABS") == null || objSub.getAttribute("ABS") == "False") 
                    {
                        // size속성이 있으면 ListBox이다.
                        if (objSub.size > 0 )
                        {
                            objSub.value = null;
                        }
                        else
                        {
                            objSub.selectedIndex = 0;                        
                        }
                    }                   
                }
                
                // 다중선택
                if (objSub.type == "select-multiple") //
                {
                    //상태, 값을 유지할 컨트롤이 아니면 기본값으로 설정
                    if (objSub.getAttribute("ABS") == null || objSub.getAttribute("ABS") == "False") 
                    {
                        objSub.value = null ;
                    }                
                }
            }             
        }
        
        //document.getElementById("hdvTest").innerHTML = strTest;
    
    
    }



    /************************************************************************
    함수명		: fn_ResetInputValue
    작성목적	: Table, Div등 컨테이너 컨트롤에 포함된 컨트롤등의 값을 초기화 합니다.
                  ASP.NET.WebControl은 모두 초기화
                  Noeplus.WebControl은 Attribute(ABS)의 설정에 따라 초기화 결정.
                                   
    Parameter 	: Table, Div등이 컨테이너 컨트롤ID

    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/      
    this.ResetInputValue = function fn_ResetInputValue(ctlID, mode) {

        // 해당 컨테이너에 포함된 컨트롤들을 모두 가져 옵니다.
        // var ctls = document.getElementById(ctlID).all;
        var ctls = document.getElementById(ctlID) ; // firefox 때문에 변경함. 2011.03.22 김윤식

        fn_ResetInputSub(ctls);
        
        return false;
    }
    
    
    /************************************************************************
    함수명		: fn_GetRowData
    작성목적	: 문자열을 json객체로 변환한다.
                                   
    Parameter 	: JSON(JavaScript Object Notation)데이타
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	: 멀티브라우져 지원 변경 2011.03.11 김윤식
    *************************************************************************/     
    this.GetRowData = function fn_GetRowData(rowData)
    {
        // rowData문자열을 Json객체로 변환
        // 프레임워크에서 json객체 문자열 생성시 [']를 사용하기 때문에 ["]변환하여 준다.
        var RegExp = /'/gi; 
        var strRowData = rowData.replace(RegExp, "\"");
        var oJson = jQuery.parseJSON(strRowData); // jQuery를 이용해서 문자열을 json으로 변환 한다. 

        for (var i = 0; i < oJson.length; i++) {
            
            oJson[i] = oJson[i].toString().replace(/＇/gi, "'").replace(/˝/gi, '"');          
        }
 
        return oJson 
 
    }
    
    /************************************************************************
    함수명		: fn_SetInputData
    작성목적	: Table, Div등 컨테이너 컨트롤에 포함된 컨트롤에 값을 대입합니다.
                  Noeplus.WebControl에 있는 SourceField를 사용합니다.
                                   
    Parameter 	: JSON(JavaScript Object Notation)데이타, Table, Div등이 컨테이너 컨트롤ID, 
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	: 멀티브라우져 지원 변경 2011.03.22 김윤식
    *************************************************************************/     
    this.SetInputData = function fn_SetInputData(rowData, ctlID) {

        try
        {
            // rowData문자열을 Json객체로 변환
            // var oJson = eval("(" + rowData + ")" );
            var oJson = WebControl.GetRowData(rowData); // 멀티브라우져 지원으로 변경 2011.03.22 김윤식
            
            // ID에 해당하는 객체를 컨트롤변수에 담는다.
            var obj = document.getElementById(ctlID);

            // 현재 컨터네컨트롤의 하위 컨트롤에 json으로 지정된 데이터의 값들을 입력한다.
            fn_childControls(oJson, obj);
            
            // 생성한 json객체를 반환하여 준다.
            return oJson ;
        
        }
        catch(e)
	    {
		    Common.NeoplusErrorMessage(e.toString(), "fn_SetInputData");
	    }
        
    }


    /************************************************************************
    함수명		: fn_childControls
    작성목적	: 컨트롤에 JSON(JavaScript Object Notation)으로 넘어온 데이터의 값을 대입합니다.
                  Noeplus.WebControl에 있는 SourceField를 사용합니다.
                                   
    Parameter 	: JSON(JavaScript Object Notation)데이타, Table, Div등이 컨테이너 컨트롤ID, 
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/  
    function fn_childControls(jsonValue, obj) {

        var strObjNodeName = "";

        try 
        {

            for (var i = 0; i < obj.childNodes.length; i++) {
        
                // 컨테이너 컨트롤의 값을 잡아온다.
                var objSub = obj.childNodes.item(i);

                // 하위에 컨트롤이 있으면 하위로 드릴다운한다.
                if (objSub.hasChildNodes) 
                {
                    fn_childControls(jsonValue, objSub);
                }
                       
                // TextBox, CheckBox, CheckBoxList, RadioButtonList, hidden에 해당하는 컨트롤의 값을 바인딩 한다.

                strObjNodeName = objSub.nodeName + ":" + objSub.id + ":" +objSub.name;

                if (objSub.nodeName == "INPUT" || objSub.nodeName == "TEXTAREA" || objSub.nodeName == "SPAN" )
                {

                    strObjNodeName += "Depth0";

                    if ( objSub.getAttribute("id") == null  || objSub.getAttribute("id") == ""  )
                    {
                        return ;
                    }


                    // 컨트롤 객체 생성
                    var ctls = document.getElementById(objSub.id);

                    strObjNodeName += "Depth1";


                    // SourceField가 있는지 확인
                    if ( ctls.getAttribute("SourceField") != undefined && ctls.getAttribute("SourceField") != null ) {
                  
                        // 소스필드가 있으면 소스필드에 해당하는 값을 가져온다.
                        dat = jsonValue[ctls.getAttribute("SourceField")];                
                              
            
                        strObjNodeName += "Depth2";

                        // hidden필드, TextBox, 멀티입력TextBox
                        if (ctls.type == "hidden" || ctls.type == "text" || ctls.type == "textarea" ) 
                        {
                            // PK가 True이면 읽기전용, 사용금지
                            if (ctls.getAttribute("PK") == "True" && ctls.getAttribute("PK") != null ) 
                            {
                                ctls.readOnly = true;
                                ctls.disabled = true;
                            }
                    
                            ctls.value = dat;
                   
                        }

                        // 체크박스에 값을 설정한다.
                        // CheckBox, CheckBoxList
                        else if (ctls.type == "checkbox")
                        {
                            // 체크박스에 값을 바인딩 한다.                    
                            WebControl.SetCheckBoxData(dat, ctls.id);
                                              
                        } 
                
                
                        // 라디오버튼 리스트
                        else if (ctls.type == "radio")
                        {
                            //WebControl.SetRadioButtonData(data, ctls.name);
                        }

                        else {
                
                            strObjNodeName += "Depth3";
                            //Div, Span일때
                            //alert(objSub.nodeName + "/" + ctls.type);

                            if (jQuery.browser.mozilla)
                            {
                                ctls.textContent = dat;
                            }
                            else
                            {
                                ctls.innerText = dat;
                            }


                        }
                    }
                
                }   
            
                // DropDownList, ListBox 바인딩
                if (objSub.nodeName == "SELECT")
                {
            
                    // 컨트롤 객체 생성
                    var ctls = document.getElementById(objSub.id);
                
                    // SourceField가 있는지 확인
                    if (ctls.getAttribute("SourceField") != undefined && ctls.getAttribute("SourceField") != null) {
                  
                        // 소스필드가 있으면 소스필드에 해당하는 값을 가져온다.
                        dat = jsonValue[ctls.getAttribute("SourceField")];
                      
                        // 단일선택
                        if (ctls.type == "select-one") //
                        {               
                
                            // PK가 True이면 읽기전용, 사용금지
                            if (ctls.getAttribute("PK") == "True" && ctls.getAttribute("PK") != null ) 
                            {
                                ctls.readOnly = true;
                                ctls.disabled = true;
                            }                    
                
                            ctls.value = dat ;
                    
                        }
                    }
                }      
         
            }

        }
        catch(e)
	    {
		    Common.NeoplusErrorMessage(e.toString(), "fn_childControls:" + strObjNodeName);
	    }

    }

    /************************************************************************
    함수명		: fn_ChrEncode
    작성목적	: 문자열을 인코딩하여 값을 리턴합니다.
    Parameter 	: 문자열
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/  
    function fn_ChrEncode(chr) {

        var o_RegExp = new RegExp("&lt;", "ig");
        chr = chr.replace(o_RegExp, "<");

        o_RegExp = new RegExp("&gt;", "ig");
        chr = chr.replace(o_RegExp, ">");

        o_RegExp = new RegExp("&lsquo;", "ig");
        chr = chr.replace(o_RegExp, "'");

        o_RegExp = new RegExp("&lt;br&gt;", "ig");
        chr = chr.replace(o_RegExp, "\n");

        return chr;
    }



    /************************************************************************
    함수명		: fn_SetCheckBoxData
    작성목적	: 체크박스 컨트롤에 데이터의 값을 대입합니다.
                  Noeplus.WebControl에 있는 SourceField를 사용합니다.
                                   
    Parameter 	: 데이타, CheckBox
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/    
    this.SetCheckBoxData = function fn_SetCheckBoxData(data, ctlID) {

        var ctl = document.getElementById(ctlID);
        if (data == "True" || data == "Y") {
            ctl.checked = true;
        }
        else if (data == "False" || data == "N" || data == "") {
            ctl.checked = false;
        }
    }
   
    /************************************************************************
    함수명		: fn_SetCheckBoxData
    작성목적	: 체크박스 컨트롤에 데이터의 값을 대입합니다.
                  Noeplus.WebControl에 있는 SourceField를 사용합니다.
                                   
    Parameter 	: 데이타 - 'Y^N^N' 또는 'True^False^True^False' 형식으로 구성되어 있으면 됨.
                 , CheckBox
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/    
    this.SetCheckBoxListData = function fn_SetCheckBoxListData(jsonValue, ctlID) {


            // CheckBoxList의 부모컨테이너(Table)객체를 찾는다.
            var oCheckBoxList = $("[id=" + ctlID + "]");
            
            // var oCheckBoxList = document.getElementById(ctlID);
            
            // SourceField가 있는지 확인
            if ( oCheckBoxList.attr("SourceField") != undefined &&  oCheckBoxList.attr("SourceField") != null) {
              
                var dataValue = jsonValue[oCheckBoxList.attr("SourceField")];
                var data = dataValue.toString().split("^");
            }
            
            // input tagName을 가진 객체중에 ctlID로 시작하는 객체를 모두 찾아옵니다.
            var oCheckBoxListItem = $("input[id^=" + ctlID + "]");

            // alert(oRadioButton.length);
            for (var i = 0; i < oCheckBoxListItem.length; i++) {

                if (data[i] == "True" || data[i] == "Y") {
                    oCheckBoxListItem[i].checked = true;
                }
                else {
                    oCheckBoxListItem[i].checked = false;
                }                     
            }

    }

    /************************************************************************
    함수명		: SetRadioButtonData(sValue, radioName)
    작성목적	: Radion 컨트롤의 이름과 값을 문자열로 전달하면 해당 컨트롤 그룹을 검색하여 해당 값을 가진 컨트롤을 체크한다.
    Parameter	:
    Return		:
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SetRadioButtonData = function fn_SetRadioButtonData(data, radioName) {

        try {

            var iCount;
            var oRGroup = document.getElementsByName(radioName);

            iCount = oRGroup.length;

            for (var i = 0; i < iCount; i++) {

                if (oRGroup[i].value == data) {
                    oRGroup[i].checked = true;
                    break;
                }
            }

        }
        catch (exception) {
        }       
    }    
    
    /************************************************************************
    함수명		: fn_SetRadioButtonListData(jsonValue, ctlID)
    작성목적	: Neoplus.WebControl.RadioButtonList객체에 값을 바인딩합니다.
                  
                  위의 객체의 경우 라디오버튼을 감싸고 있는 부모컨테이너(Table)객체에 
                  ABS, Required, SourceField, Description의 속성이 지정된다.

                  그러므로 해당 속성을 활용하여 데이터를 바인딩하려면 이 함수를 사용하면 자동 바인딩 기능을 활용할 수 있습니다.
    Parameter	:
    Return		:
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.SetRadioButtonListData = function fn_SetRadioButtonListData(jsonValue, ctlID) {

        try {

                var oRadioButton = $("[id=" + ctlID + "]");
               
                // SourceField가 있는지 확인
                if ( oRadioButton.attr("SourceField") != undefined &&  oRadioButton.attr("SourceField") != null) {
                  
                    // 소스필드가 있으면 소스필드에 해당하는 값을 가져온다.
                    data = jsonValue[ oRadioButton.attr("SourceField")].toString();                
                }                
                
                // input tagName을 가진 객체중에 ctlID로 시작하는 객체를 모두 찾아옵니다.
                var oRadioButtonListItem = $("input[id^=" + ctlID + "]");

                // alert(oRadioButton.length);
                for (var i = 0; i < oRadioButtonListItem.length; i++) {

                    if (oRadioButtonListItem[i].value == data) {
                        oRadioButtonListItem[i].checked = true;
                        break;
                    }
                }                
        }
        catch (exception) {
        }       
    } 
    
    
    /************************************************************************
    함수명		: fn_SelectAll
    작성목적	: ListView에 속한 CheckBox에 check, uncheck를 처리한다.
    Parameter	: ListView(Table)객체, checkBox명, check, uncheck 처리
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    function fn_SelectAll(frm, chkName, checked) {
    
        for (var i = 0; i < frm.elements.length; i++) 
        {
            var e = frm.elements[i];
        
            if (e.type == "checkbox" && e.name.indexOf(chkName) >= 0 && !e.disabled) // 비활성화된 것 제외
                e.checked = checked;
        }
    }

    /************************************************************************
    함수명		: fn_MultiCheck
    작성목적	: ListView에 속한 CheckBox에 check, uncheck를 처리한다.
    Parameter	: ListView(Table)객체, checkBox명, check, uncheck 처리
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	:
    *************************************************************************/
    this.MultiCheck = function fn_MultiCheck(chkName, checked) {

        var frm = document.forms[0];
        fn_SelectAll(frm, chkName, checked);
    }


    /************************************************************************
    함수명		: fn_FocusRow
    작성목적	: 리스트에서 마우스오버, 아웃시 색을 변경해 줍니다.
    Parameter	: obj:tr객체, type:over, out
    사용예제    : <tr style="cursor:hand" onmouseover="fn_FocusRow(this, 'over');" onmouseout="fn_FocusRow(this, 'out');">  
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	: 2011.03.22 멀티브라우져 지원 변경 김윤식
    *************************************************************************/
    this.SelectedRow = function fn_SelectedRow(obj) {

        // selected 속성이 없으면 맹글어 준다.
        if (obj.getAttribute("selected") == null) {
            obj.setAttribute("selected", "true");
        }

        // 테이블 전체열 배경을 흰색으로 적용한후 선택한 열만 색상 적용함
        
        // var parent = obj.parentElement;
        var parent = obj.parentNode;  // FF가 지원하도록 변경

        var children = parent.children ;
        var child ;

        for (var n1 = 0; n1 < children.length; n1++) {
            child = children[n1];

            if (child.getAttribute("selected") == 'true') {
                child.setAttribute("selected", 'false' );
                this.FocusRow(child, 'out');
            }
        }

        // 선택한 로우만 배경색을 바꿔준다.
        obj.setAttribute("selected", 'true');
        obj.className = "tr_select";
    }


    /************************************************************************
    함수명		: fn_FocusRow
    작성목적	: 리스트에서 마우스오버, 아웃시 색을 변경해 줍니다.
    Parameter	: obj:tr객체, type:over, out
    사용예제    : <tr style="cursor:hand" onmouseover="fn_FocusRow(this, 'over');" onmouseout="fn_FocusRow(this, 'out');">  
    Return		: 
    작 성 자	: (주)네오플러스 김윤식
    최초작성일	: 2010.03.12
    최종작성일	:
    수정내역	: 2011.03.22 멀티브라우져 지원 김윤식
    *************************************************************************/
    this.FocusRow = function fn_FocusRow(obj, type) {

        // selected 속성이 없으면 맹글어 준다.
        if (obj.getAttribute("selected") == null) {
            obj.setAttribute("selected", "false");
        }

        // 선택된 상태가 아니라면
        if (obj.getAttribute("selected") != 'true') {

            // 마우스가 올라갔을때
            if (type == 'over') {
                obj.className = "tr_over";
            }
            else {
                // 마우스가 나갈때
                if (obj.getAttribute("selected") == 'true') {
                    // 현재 Row가 선택된 상황이라면
                    obj.className = "tr_select";
                }
                else {
                    if (obj.getAttribute("isalternat") == '0') {
                        obj.className = "";
                    }
                    else {
                        obj.className = "tr_bg";
                    }
                }
            }
        }

    }           
    
}

// 스크립트 선언
var WebControl = new NeoplusWebControl();
