var hebeiCtrls = angular.module('hebeiCtrls',['hebeiService','ngCookies','hebeiDirectives','hebeiFilters','tm.pagination']);
hebeiCtrls.controller("Config",function($scope,$http,$cookies,$cookieStore,$rootScope){
    $scope.HttpUrl = "http://101.200.210.193:7465/";
 //   $scope.HttpUrl = "http://localhost:12094/";
    $scope.loginame = $cookies.get("user");
    $scope.token = $cookies.get("LoginToken");
    $scope.type = $cookies.get("type");
});

hebeiCtrls.controller('head',function($scope,$http,$cookies,$cookieStore,$rootScope){
	// $scope.$on('to-child',function(event,data) {
 //        console.log('Config',$rootScope.menudata);       //
 //        $scope.MenuInfo = $rootScope.menudata;
 //    });
 	$scope.menu = function(token,type){
 		var getmenu = $.ajax({
 			type:'GET',
 			url:$scope.HttpUrl +'MenuInfo'+'?token='+token,
 			data:{
 				AccountType:type,
 			},
 			dataType:'json'
 		});
 		getmenu.success(function(data,status,headers,config){
 			$scope.$apply(function(){
 				$scope.menuinfo = data;
 			});
 			console.log(data.length);
 			console.log(data);
 			console.log(data[0].ParentID);
 		});
 		getmenu.error(function(responese,status,headers,config){
 			//alert("菜单栏读取失败！");
 		})
 	};
	$scope.exit = function(token){
		var exituser = $.ajax({
			type:'POST',
			headers:{
				'Content-Type': 'application/x-www-form-urlencoded'
		  	},
			url: $scope.HttpUrl +'Login'+'?token='+token,
			data:{
				Remark:token,
				HttpMethod:'DELETE'
			},
			dataType:'json'
		});
		exituser.success(function(data,status,headers,config){
			$cookies.remove("LoginToken");
			$cookies.remove("user");
			$cookies.remove("type");
			location.href="login.html";
		});
		exituser.error(function(responese,status,headers,config){
			console.log(responese);  //失败的状态号
			console.log(status);
			console.log(headers);
		});
	};
});
hebeiCtrls.controller('body',['$scope',
	function($scope){
		$scope.companies=[
			{name:"123321",water:"133",air:"112",level:"1"},
			{name:"123qwe",water:"113",air:"132",level:"2"},
			{name:"1we2e1",water:"321",air:"142",level:"3"},
			{name:"13we21",water:"111",air:"1a2",level:"4"}
		],
		$scope.warnings=[
			{degree:"严重！",describe:"028号大气传感器通讯失败",type:"danger"},
			{degree:"严重！",describe:"水质酸碱度超标.",type:"info"},
			{degree:"严重！",describe:"水质酸碱度超标. ",type:"info"},
			{degree:"严重！",describe:"大气污染严重超标. ",type:"warning"}
		],
		$scope.news=[
			{href:"#",title:"新闻1号"},
			{href:"#",title:"新闻2号"},
			{href:"#",title:"新闻3号"},
		],
		$scope.acompanynames=[
			{href:"#",name:"A公司"},
			{href:"#",name:"B公司"},
			{href:"#",name:"C公司"},
			{href:"#",name:"D公司"}
		],
		$scope.bcompanynames=[
			{href:"#",name:"E公司"},
			{href:"#",name:"F公司"},
			{href:"#",name:"G公司"},
			{href:"#",name:"H公司"}
		]
	}
]);
hebeiCtrls.controller('foot',function($scope){
});
hebeiCtrls.controller('Login',function($scope,$http,$cookies,$cookieStore,$rootScope){
		$scope.getLogin = {
			Username:"wangzhao",
			Password:"123"
		};
		$scope.Login = function(getLogin){
			var login1 = $.ajax({
				url: $scope.HttpUrl + 'Login',
				type:'POST',
				// headers:{
				// 	'Content-Type': 'application/x-www-form-urlencoded'
  	        	// },
  				data:{
 					LoginName: getLogin.Username,
 					PSW: getLogin.Password,
 					HttpMethod:'POST'
  				},
  				dataType:'json'
			});
		login1.success(function(data,status,headers,config){
			$scope.loginfo = data;
			console.log(headers.status);    //成功的状态号
			$scope.$apply(function(){                  //
				$scope.MenuInfo = data.MenuInfo;
			});
			$cookies.put("type",data.AccountType);
			$cookies.put("LoginToken",data.LoginToken);
			$cookies.put("user",data.LoginName);

			location.href="index.html";


		});
		login1.error(function(responese,status,headers,config){
			console.log(responese.status);  //失败的状态号
			console.log(status);
			console.log(headers);
			console.log($scope.HttpUrl);
		});
		//从服务器得到json。
		};
});
hebeiCtrls.controller('water1',function($scope){
	
	$scope.amount="污水量巨大"
});
hebeiCtrls.controller('water2',function($scope){
	$scope.wastewaters=[
		{name:"A点",hg:"5",gc:"3",cd:"5",cr:"2",amount:"12"},
		{name:"B点",hg:"2",gc:"1",cd:"1",cr:"8",amount:"22"},
		{name:"C点",hg:"1",gc:"5",cd:"2",cr:"9",amount:"31"},
		{name:"D点",hg:"5",gc:"4",cd:"4",cr:"4",amount:"45"},
		{name:"E点",hg:"4",gc:"6",cd:"1",cr:"1",amount:"37"},
		{name:"F点",hg:"1",gc:"6",cd:"2",cr:"5",amount:"18"},
		{name:"G点",hg:"5",gc:"6",cd:"5",cr:"5",amount:"42"}
	],
	$scope.quarterwaters=[
		{quarter:"第一季度",hg:"5",gc:"3",cd:"1",cr:"5",amount:"14"},
		{quarter:"第二季度",hg:"2",gc:"2",cd:"5",cr:"2",amount:"21"},
		{quarter:"第三季度",hg:"1",gc:"4",cd:"2",cr:"6",amount:"12"},
		{quarter:"第四季度",hg:"4",gc:"3",cd:"5",cr:"2",amount:"36"}
	]
});
hebeiCtrls.controller('water3',function($scope){
});
hebeiCtrls.controller('water4',function($scope){
});

hebeiCtrls.controller('UserManagement',function($scope,$http,$cookies,$cookieStore,$rootScope){
	$scope.paginationConf = {
		totalItems: 20,
		currentPage: 1,
		itemsPerPage: 10,
		pagesLength: 15,
		perPageOptions: [10, 20, 30, 40, 50],
		onChange: function(){
			var Show1 = $.ajax({
				type:'GET',
				url: $scope.HttpUrl +'user'+'?token='+token,
				data:{
					currentPage:$scope.paginationConf.currentPage,
					itemsPerPage:$scope.paginationConf.itemsPerPage,

				},
				dataType:'json',
			});
			Show1.success(function(data,status,headers,config){
				$scope.$apply(function(){
					$scope.UserData = data.model;
					$scope.paginationConf.totalItems = data.Count;
				});
				console.log(data);
			});
			Show1.error(function(responese,status,headers,config){
				alert(responese.responseJSON);
				location.href = "login.html";
				console.log(responese.responseJSON);
				if (responese.status == 401) {
					alert:"用户验证已过期";
					location.href="login.html"
				}
				console.log(status);
				console.log(headers);
				console.log(config);
			});
		}

	};

	$scope.User = {
		LoginName:"",
		PSW:"",
		Name:"",
		Gender:"",
		Tel:"",
		Emai:"",
		AccountType:"",
		HttpMethod:""
	};
	$scope.one = [
		{
			id:'1',
			gender:'男'
		},
		{
			id:'2',
			gender:'女'
		}
	];
	$scope.two = [
		{
			id:'1',
			type:'管理员'
		},
		{
			id:'2',
			type:'园区领导'
		},
		{
			id:'3',
			type:'企业领导'
		},
		{
			id:'4',
			type:'园区管理员'
		},
		{
			id:'5',
			type:'企业一般管理员'
		},
	];
	var token = $scope.token;
	if (token == null)
	{
		alert("请先登录！");
		location.href = "login.html";
	}
	else
	{
		$scope.Show = function(getUser){                         //查
			var Show1 = $.ajax({
				type:'GET',
				url: $scope.HttpUrl +'user'+'?token='+token,
				data:{									
				},
				dataType:'json',
			});
			Show1.success(function(data,status,headers,config){
				$scope.$apply(function(){
					$scope.UserData = data;

				});
				console.log(data);
			});
			Show1.error(function(responese,status,headers,config){
				alert(responese.responseJSON);
				location.href = "login.html";
				console.log(responese.responseJSON);
			if (responese.status == 401) {
				alert:"用户验证已过期";
				location.href="login.html"
			}
				console.log(status);
				console.log(headers);
				console.log(config);
			});
		};
	};

	$scope.InsertUser = function(User){                      //增
		var InsertUser1 = $.ajax({
			type:'POST',
			headers:{
				'Content-Type': 'application/x-www-form-urlencoded'
  			},
			url: $scope.HttpUrl +'user'+'?token='+token,
			data:{
				LoginName: User.LoginName,
				PSW: User.PSW,
				Name: User.Name,
				Gender: User.Gender,
				Tel: User.Tel,
				Email:User.Email,
				AccountType: User.AccountType,
				HttpMethod:'POST'
			},
			dataType:'json'
		});
		InsertUser1.success(function(data,status,headers,config){
			alert("保存成功！");
			console.log(status);
			window.location.reload(true);
		});
		InsertUser1.error(function(response,status,headers,config){
			console.log(response.responseJSON);
			console.log(status);
		});
	};
	$scope.SaveUser = function(x){                                      //改
		var Update = $.ajax({
			type:'POST',
			url: $scope.HttpUrl +'user'+'?token='+token,
			data:{
				ID: x.ID,
				LoginName: x.LoginName,
				PSW: x.PSW,
				Name: x.Name,
				Gender: x.Gender,
				Tel: x.Tel,
				Email: x.Email,
				AccountType: x.AccountType,
				HttpMethod:'PUT'
			},
			dataType:'json'
		});
		Update.success(function(data,status,headers,config){
			alert("修改成功！");
			//console.log(status);
			window.location.reload(true);
		});
		Update.error(function(response,status,headers,config){
			alert("error");
			console.log(response);
			console.log(status);
			console.log(x);
		});
	};
	$scope.DeleteUser = function(x){                      //删
        var ids="";
        var flag;
        $("input:checkbox:checked").each(function(){                                    
            flag=true;
            ids+=$(this).val()+",";
        })
        if(!flag){
            alert("请至少选择一项");
        }
        if(ids.length > 0)
        	if(confirm("是否删除所选条目？"))
        	{
        		ids = ids.substring(0, ids.length - 1); 
        		var Delete =  $.ajax({
        		    type: 'POST',
        		    url: $scope.HttpUrl +"user"+"?token="+token,
        		    data:{
        		        "Remark": ids,
        		        "HttpMethod":"DELETE",
        		    },
        		    dataType: 'json',
        		});
        		Delete.success(function(data,status,headers,config){
        		    alert("删除成功");
        		    //show(getShow);
        		    window.location.reload(true);
        		});
        		Delete.error(function(responese,status,headers,config){
        		    console.log(responese);
        		    alert("操作失败");
        		});
        	};
    };
});
hebeiCtrls.controller('CompanyManagment',function($scope,$http,$cookies,$cookieStore){
	$scope.Company = {
		Name:"",
		Address:"",
		Tel:"",
		Email:"",
		CreateTime:"",
		LastUpdateTime:""
	};
	var token = $scope.token;
	if (token == null) 
	{
		alert("请先登录！");
		location.href = "login.html";
	}
	else
	{
		$scope.showCompany = function(getCompany){
			var Show2 = $.ajax({
				type:'GET',
				url:$scope.HttpUrl +'company'+'?token'+token,
				data:{
				},
				dataType:'json',
			});
			Show2.success(function(data,status,headers,config){
				console.log(data);
				$scope.$apply(function(){
					$scope.CompanyData = data;
				});
			});
			Show2.error(function(response,status,headers,config){
				alert(responese.responseJSON);
				location.href = "login.html";
				console.log(responese.responseJSON);
				if (responese.status == 401) {
					alert:"用户验证已过期";
					location.href="login.html"
				};
			});
		}
	};
	$scope.InsertCompany = function(Company){
		var time = new Date();
		var year = time.getFullYear();
		var month = time.getMonth()+1;
		var date = time.getDate();
		var hour = time.getHours();
		var minute = time.getMinutes();
		var second = time.getSeconds();
		$scope.Company.CreateTime = year + '-' + month + '-' + date + 'T' + hour + ':' + minute + ':' + second;
		$scope.Company.LastUpdateTime = $scope.Company.CreateTime;
		var InsertCompany1 = $.ajax({
			type:'POST',
			headers:{
				'Content-Type': 'application/x-www-form-urlencoded'
  			},
  			url: $scope.HttpUrl +'company'+'?token='+token,
  			data:{
  				CompanyName: Company.Name,
  				CompanyAddress: Company.Address,
  				Tel: Company.Tel,
  				Email: Company.Email,
  				CreateTime: Company.CreateTime,
  				LastUpdateTime: Company.LastUpdateTime,
  				HttpMethod:'POST'
  			},
  			dataType:'json'
		});
		InsertCompany1.success(function(data,status,headers,config){
			console.log($scope.Company.CreateTime);
			window.location.reload(true);
		});
		InsertCompany1.error(function(response,status,headers,config){
			console.log(now);
		})
	};
	$scope.deleteCompany = function(x){
		var ids="";
		var flag;
		$("input:checkbox:checked").each(function(){                                    
		    flag=true;
		    ids+=$(this).val()+",";
		})
		if(!flag){
		    alert("请至少选择一项");
		}
		if(ids.length > 0)
		{
			if(confirm("是否删除所选条目？"))
			{
				ids = ids.substring(0, ids.length - 1); 
				var Delete =  $.ajax({
				    type: 'POST',
				    url: $scope.HttpUrl +"Company"+"?token="+token,
				    data:{
				        "Remark": ids,
				        "HttpMethod":"DELETE",
				    },
				    dataType: 'json',
				});
				Delete.success(function(data,status,headers,config){
				    alert("删除成功");
				    window.location.reload(true);
				});
				Delete.error(function(responese,status,headers,config){
				    console.log(responese);
				    alert("操作失败");
				});
			};
		};
	};
	$scope.saveCompany = function(x){
			var time = new Date();
			var year = time.getFullYear();
			var month = time.getMonth();
			var date = time.getDate();
			var hour = time.getHours();
			var minute = time.getMinutes();
			var second = time.getSeconds();
			var LastUpdateTime = year + '-' + month + '-' + date + 'T' + hour + ':' + minute + ':' + second;
			var Update = $.ajax({
			type:'POST',
			url: $scope.HttpUrl +'company'+'?token='+token,
			data:{
				ID: x.ID,
				CompanyName: x.CompanyName,
  				CompanyAddress: x.CompanyAddress,
  				Tel: x.Tel,
  				Email: x.Email,
  				CreateTime: x.CreateTime,
  				LastUpdateTime: LastUpdateTime,
  				HttpMethod:'PUT'
			},
			dataType:'json'
		});
		Update.success(function(data,status,headers,config){
			alert("修改成功！");
			console.log(LastUpdateTime);
			//window.location.reload(true);
		});
		Update.error(function(response,status,headers,config){
			alert("error");
			console.log(response);
			console.log(status);
			console.log(x);
		});
	}
});
hebeiCtrls.controller('air1',function($scope){
});
hebeiCtrls.controller('air2',function($scope){
});
hebeiCtrls.controller('air3',function($scope){
});
hebeiCtrls.controller('air4',function($scope){
});
hebeiCtrls.controller('OverproofAnalysis',function($scope){
});
hebeiCtrls.controller('PollutionAnalysis',function($scope){
});
hebeiCtrls.controller('Reporter',function($scope){
});
hebeiCtrls.controller('msgPushSetting',function($scope,$http,$cookies,$cookieStore){
	$scope.PushSetting = {
		CreateTime:"",      //创建时间
		ValidityTime:"",	//持续时间
		MessageContent:"",  //内容
		MessageType:""
	};
	//$scope.abs ="";
	$scope.one = [
		{
			id:'0.5',
			day:'0.5天'
		},
		{
			id:'1',
			day:'1天'
		},
		{
			id:'1.5',
			day:'1.5天'
		},
		{
			id:'2',
			day:'2天'
		},
		{
			id:'2.5',
			day:'2.5天'
		},
		{
			id:'3',
			day:'3天'
		}
	];
	$scope.two = [
		{
			id:'1',
			info:'短信推送'
		},
		{
			id:'2',
			info:'水推送'
		},
		{
			id:'3',
			info:'废气推送'
		},
		{
			id:'4',
			info:'预警决策'
		},
		{
			id:'5',
			info:'设备'
		},
		{
			id:'6',
			info:'报警信息'
		}
	];
	var token = $scope.token;
	if (token == null)
	{
		alert("请先登录！");
		location.href = "login.html";
	}
	else
	{
		$scope.showPushSetting = function(getPushSetting){
			var show3 = $.ajax({
				type:'GET',
				url:$scope.HttpUrl +'PushData'+'?token'+token,
				data:{
				},
				dataType:'json',
			});
			show3.success(function(data,status,headers,config){
				$scope.$apply(function(){
					$scope.SettingData = data;
				});
				console.log($scope.SettingData);
			});
			show3.error(function(response,status,headers,config){
				console.log(response);
			});
		};
	};
	$scope.deletePushSetting = function(x){
		var ids="";
		var flag;
		$("input:checkbox:checked").each(function(){                                    
		    flag=true;
		    ids+=$(this).val()+",";
		})
		if(!flag){
		    alert("请至少选择一项");
		}
		if(ids.length > 0)
		{
			if(confirm("是否删除所选条目？"))
			{
				ids = ids.substring(0, ids.length - 1); 
				var Delete =  $.ajax({
				    type: 'POST',
				    url: $scope.HttpUrl +"PushData"+"?token="+token,
				    data:{
				        "Remark": ids,
				        "HttpMethod":"DELETE",
				    },
				    dataType: 'json',
				});
				Delete.success(function(data,status,headers,config){
				    alert("删除成功");
				    window.location.reload(true);
				});
				Delete.error(function(responese,status,headers,config){
				    console.log(responese);
				    alert("操作失败");
				});
			};
		};
	};
	$scope.addPushSetting = function(PushSetting){
		var time = new Date();
		var year = time.getFullYear();
		var month = time.getMonth() + 1;
		var date = time.getDate();
		var hour = time.getHours();
		var minute = time.getMinutes();
		var second = time.getSeconds();
		$scope.PushSetting.CreateTime = year + '-' + month + '-' + date + 'T' + hour + ':' + minute + ':' + second;
		var add1 = $.ajax({
			type:'POST',
			headers:{
				'Content-Type': 'application/x-www-form-urlencoded'
  			},
  			url: $scope.HttpUrl +'PushData'+'?token='+token,
  			data:{
  				CreateTime: PushSetting.CreateTime,
  				ValidityTime: PushSetting.ValidityTime,
  				MessageContent: PushSetting.MessageContent,
  				MessageType: PushSetting.MessageType,
  				HttpMethod:'POST'
  			},
  			dataType:'json'
		});
		add1.success(function(data,status,headers,config){
			alert("保存成功！");
			console.log(PushSetting);
			//window.location.reload(true);
		});
		add1.error(function(response,status,headers,config){
			console.log(response.responseJSON);
			console.log(status);
		});
	}
});
hebeiCtrls.controller('EquipmentManagment',function($scope,$http,$cookies,$cookieStore){
	$scope.Device = {
		Number:"",
		Name:"",
		Status:"",
		Type:"",
		PlantNumber:"",
		Address:"",
		Person:"",
		Tel:"",
		Phone:""
	};
	$scope.Calibration = {
		DeviceNumber:"",
		DeviceCalibrationTime:"",
		DeviceCalibrationPsrson:"",
		DeviceAddress:""
	};
	var token = $scope.token;
	if (token == null)
	{
		alert("请先登录！");
		location.href = "login.html";
	}
	else
	{
		$scope.showDevice = function(getDevice){
			var Show = $.ajax({
				type:'GET',
				url: $scope.HttpUrl +'Device'+'?token='+token,
				data:{
				},
				dataType:'json'
			});
			Show.success(function(data,status,config,headers){
				$scope.$apply(function(){
					$scope.DeviceData = data;
				});
			});
			Show.error(function(response,headers,status,config){
				if (responese.status == 401) {
				alert:"用户验证已过期";
				location.href="login.html"
				}
				else
				{
					alert:"error";
				}
			})
		};
		$scope.showCalibration = function(getCalibration){
			var Show = $.ajax({
				type:'GET',
				url: $scope.HttpUrl +'DeviceCalibration'+'?token='+token,
				data:{
				},
				dataType:'json'
			});
			Show.success(function(data,status,config,headers){
				$scope.$apply(function(){
					$scope.CalibrationData = data;
				});
			});
			Show.error(function(response,headers,status,config){
				if (responese.status == 401) {
				alert:"用户验证已过期";
				location.href="login.html"
				}
				else
				{
					alert:"error";
				}
			})
		};
	};
	$scope.InsertDevice = function(Device){
		var Insert = $.ajax({
			type: 'POST',
			url: $scope.HttpUrl +'Device'+'?token='+token,
			headers:{
				'Content-Type': 'application/x-www-form-urlencoded'
  			},
			data:{
				DeviceNumber: Device.Number,
				DeviceName: Device.Name,
				DeviceStatus: Device.Status,
				DeviceType: Device.Type,
				DevicePlantNumber: Device.PlantNumber,
				DeviceAddress: Device.Address,
				Person:	Device.Person,
				Tel: Device.Tel,
				Phone: Device.Phone,
				HttpMethod:'POST'
			},
			dataType:'json'
		});
		Insert.success(function(data,config,headers,status){
			console.log($scope.data);
			window.location.reload(true);
		});
		Insert.error(function(response,config,headers,status){
			alert("error");
		});
	};
	$scope.DeleteDevice = function(x){                      //删
        var ids="";
        var flag;
        $("input:checkbox:checked").each(function(){                                    
            flag=true;
            ids+=$(this).val()+",";
        })
        if(!flag){
            alert("请至少选择一项");
        }
        if(ids.length > 0)
        	if(confirm("是否删除所选条目？"))
        	{
        		ids = ids.substring(0, ids.length - 1); 
        		var Delete =  $.ajax({
        		    type: 'POST',
        		    url: $scope.HttpUrl +"Device"+"?token="+token,
        		    data:{
        		        "Remark": ids,
        		        "HttpMethod":"DELETE",
        		    },
        		    dataType: 'json',
        		});
        		Delete.success(function(data,status,headers,config){
        		    alert("删除成功");
        		    //show(getShow);
        		    window.location.reload(true);
        		});
        		Delete.error(function(responese,status,headers,config){
        		    console.log(responese);
        		    alert("操作失败");
        		});
        	};
    };
    $scope.SaveDevice = function(x){                                      //改
    	var Update = $.ajax({
    		type:'POST',
    		url: $scope.HttpUrl +'Device'+'?token='+token,
    		data:{
    			ID: x.ID,
    			DeviceNumber: x.DeviceNumber,
    			DeviceName: x.DeviceName,
    			DeviceStatus: x.DeviceStatus,
    			DeviceType: x.DeviceType,
    			DevicePlantNumber: x.DevicePlantNumber,
    			DeviceAddress: x.DeviceAddress,
    			Person:	x.Person,
    			Tel: x.Tel,
    			Phone: x.Phone,
    			HttpMethod:'PUT'
    		},
    		dataType:'json'
    	});
    	Update.success(function(data,status,headers,config){
    		alert("修改成功！");
    		//console.log(status);
    		window.location.reload(true);
    	});
    	Update.error(function(response,status,headers,config){
    		alert("error");
    		console.log(response);
    		console.log(status);
    		console.log(x);
    	});
    };
    $scope.InsertCalibration = function(Calibration){
    	var time = new Date();
    	var year = time.getFullYear();
    	var month = time.getMonth() + 1;
    	var date = time.getDate();
    	var hour = time.getHours();
    	var minute = time.getMinutes();
    	var second = time.getSeconds();
    	$scope.Calibration.DeviceCalibrationTime = year + '-' + month + '-' + date + 'T' + hour + ':' + minute + ':' + second;
    	var Insert = $.ajax({
    		type: 'POST',
    		url: $scope.HttpUrl +'DeviceCalibration'+'?token='+token,
    		headers:{
    			'Content-Type': 'application/x-www-form-urlencoded'
    	  	},
    		data:{
    			DeviceNumber: Calibration.DeviceNumber,
    			DeviceCalibrationTime: Calibration.DeviceCalibrationTime,
    			DeviceCalibrationPsrson: Calibration.DeviceCalibrationPsrson,
    			DeviceAddress: Calibration.DeviceAddress,
    			HttpMethod:'POST'
    		},
    		dataType:'json'
    	});
    	Insert.success(function(data,config,headers,status){
    		console.log($scope.data);
    		window.location.reload(true);
    	});
    	Insert.error(function(response,config,headers,status){
    		alert("error");
    	});
    };
    $scope.DeleteCalibration = function(y){                      //删
        var ids="";
        var flag;
        $("input:checkbox:checked").each(function(){                                    
            flag=true;
            ids+=$(this).val()+",";
        })
        if(!flag){
            alert("请至少选择一项");
        }
        if(ids.length > 0)
        	if(confirm("是否删除所选条目？"))
        	{
        		ids = ids.substring(0, ids.length - 1); 
        		var Delete =  $.ajax({
        		    type: 'POST',
        		    url: $scope.HttpUrl +"DeviceCalibration"+"?token="+token,
        		    data:{
        		        "Remark": ids,
        		        "HttpMethod":"DELETE",
        		    },
        		    dataType: 'json',
        		});
        		Delete.success(function(data,status,headers,config){
        		    alert("删除成功");
        		    //show(getShow);
        		    window.location.reload(true);
        		});
        		Delete.error(function(responese,status,headers,config){
        		    console.log(responese);
        		    alert("操作失败");
        		});
        	};
    };
    $scope.SaveCalibration = function(y){                                     //改
    	var Update = $.ajax({
    		type:'POST',
    		url: $scope.HttpUrl +'DeviceCalibration'+'?token='+token,
    		data:{
    			ID: y.ID,
    			DeviceNumber: y.DeviceNumber,
    			DeviceCalibrationTime: y.DeviceCalibrationTime,
    			DeviceCalibrationPsrson: y.DeviceCalibrationPsrson,
    			DeviceAddress: y.DeviceAddress,
    			HttpMethod:'PUT'
    		},
    		dataType:'json'
    	});
    	Update.success(function(data,status,headers,config){
    		alert("修改成功！");
    		console.log(data);
    		//console.log(status);
    		window.location.reload(true);
    	});
    	Update.error(function(response,status,headers,config){
    		alert("error");
    		console.log(response);
    		console.log(status);
    		console.log(y);
    	});
    };
});
hebeiCtrls.controller('mailPushSetting',function($scope,$http,$cookies,$cookieStore){
	$scope.PushSetting = {
		CreateTime:"",
		EmailTitle:"",
		EmailBody:"",
		Remark:"",
		CreateTime:""
	};
	$scope.one = [
		{
			id:'1',
			type:'管理员'
		},
		{
			id:'2',
			type:'园区领导'
		},
		{
			id:'3',
			type:'企业领导'
		},
		{
			id:'4',
			type:'园区管理员'
		},
		{
			id:'5',
			type:'企业一般管理员'
		},
	];
	var token = $scope.token;
	if (token == null)
		{
			alert("请先登录！");
			location.href = "login.html";
		}
	else
		{
			$scope.showEmail = function(getEmail){
				var show = $.ajax({
					type:'GET',
					url:$scope.HttpUrl +'PushEmail'+'?token'+token,
					data:{
					},
					dataType:'json',
				});
				show.success(function(data,status,headers,config){
					$scope.$apply(function(){
						$scope.MailData = data;
					});
					console.log($scope.MailData);
				});
				show.error(function(response,status,headers,config){
					console.log(response);
				});
			};
		};
	$scope.InsertEmail = function(PushSetting){
		var time = new Date();
		var year = time.getFullYear();
		var month = time.getMonth() + 1;
		var date = time.getDate();
		var hour = time.getHours();
		var minute = time.getMinutes();
		var second = time.getSeconds();
		$scope.PushSetting.CreateTime = year + '-' + month + '-' + date + 'T' + hour + ':' + minute + ':' + second;
		var Insert = $.ajax({
			type: 'POST',
			url: $scope.HttpUrl +'PushEmail'+'?token='+token,
			headers:{
				'Content-Type': 'application/x-www-form-urlencoded'
  			},
			data:{
				EmailTitle: PushSetting.EmailTitle,
				EmailBody: PushSetting.EmailBody,
				Remark: PushSetting.Remark,
				CreateTime: PushSetting.CreateTime,
				HttpMethod:'POST'
			},
			dataType:'json'
		});
		Insert.success(function(data,config,headers,status){
			console.log($scope.data);
			window.location.reload(true);
		});
		Insert.error(function(response,config,headers,status){
			alert("error");
		});
	}
});