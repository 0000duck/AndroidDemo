//var app = angular.module('app', ['appService', 'ngCookies', 'appDirectives', 'appFilters', 'tm.pagination']);


app.controller("Config", function ($scope, $http, $cookies, $cookieStore, $rootScope) {
	$scope.HttpUrl = "http://101.200.210.193:7465/";
	//$scope.HttpUrl = "http://localhost:12094/";
    $scope.loginame = $cookies.get("user");
    $scope.token = $cookies.get("LoginToken");
    $scope.type = $cookies.get("type");
	$scope.Company = $cookies.get("Company");
});
app.controller('head', function ($scope, $http, $cookies, $cookieStore, $rootScope) {
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
app.controller('body', ['$scope', function ($scope) {
	$(document).ready(function () {
     Createradar("radar1");
     CreatePie("pie1");
});
	function Createradar(divId)
    {
        var dom = document.getElementById(divId);
        var myChartR = echarts.init(dom);
        option = null;

        option = {
        title : {
            text: '衡水工业新区污染雷达图',
            subtext: '污染排放贡献'
        },
        tooltip : {
            trigger: 'axis'
        },
        legend: {
            x : 'center',
            data:['水污染','大气污染']
        },
        toolbox: {
            show : false,
            feature : {
                mark : {show: true},
                dataView : {show: false, readOnly: false},
                restore : {show: true},
                saveAsImage : {show: true}
            }
        },
        calculable : true,
        polar : [
            {
                indicator : [
                    {text : '冀衡药业', max  : 100},
                    {text : '冠龙农化', max  : 100},
                    {text : '衡林生物', max  : 100},
                    {text : '冀衡化肥', max  : 100},
                    {text : '蓝天化工', max  : 100},
                    {text : '冀衡化学', max  : 100},
                    {text : '东华化工', max  : 100},
                    {text : '东北助剂', max  : 100}
                ],
                radius : 130
            }
        ],
        series : [
            {
                name: '污染排放贡献',
                type: 'radar',
                itemStyle: {
                    normal: {
                        areaStyle: {
                            type: 'blue'
                        }
                    }
                },
                data : [
                    {
                        value : [57, 42, 88, 34, 40, 36, 60, 35],
                        name : '水污染'
                    },
                    {
                        value : [47, 32, 74, 65, 28, 52, 52, 18],
                        name : '大气污染'
                    }
                 ]
              }
           ]
        };

     myChartR.setOption(option, true);
    }


    function CreatePie(divId)
    {
        var dom = document.getElementById(divId);
        var myChartP = echarts.init(dom);
        option = null;

        option = {
            title : {
                text: '园区污染前五企业比例图',
                subtext: '河北衡水工业新区',
                x:'center'
            },
            tooltip : {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: ['衡林生物','冀衡化学','蓝天化工','东华化工','东北助剂']
            },
            series : [
                {
                    name: '污染排放',
                    type: 'pie',
                    radius : '55%',
                    center: ['50%', '60%'],
                    data:[
                        {value:335, name:'衡林生物'},
                        {value:310, name:'冀衡化学'},
                        {value:234, name:'蓝天化工'},
                        {value:135, name:'东华化工'},
                        {value:1548, name:'东北助剂'}
                    ],
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };

     myChartP.setOption(option, true);
    }
	
	
		$scope.paginationConf = {
			totalItems: 20,
			currentPage: 1,
			itemsPerPage: 3,
			pagesLength: 15,
			perPageOptions: [3],
			onChange: function(){
				var Show1 = $.ajax({
					type:'GET',
					url: $scope.HttpUrl +'News'+'?token=',
					data:{
						currentPage:$scope.paginationConf.currentPage,
						itemsPerPage:$scope.paginationConf.itemsPerPage,

					},
					dataType:'json',
				});
				Show1.success(function(data,status,headers,config){
					$scope.$apply(function(){
						$scope.NewsData = data.model;
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
app.controller('foot', function ($scope) {
});
app.controller('Login', function ($scope, $http, $cookies, $cookieStore, $rootScope) {
		$scope.getLogin = {
			Username:"chenwenji",
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
			console.log(data);
			$scope.loginfo = data;
			console.log(headers.status);    //成功的状态号
			$scope.$apply(function(){                  //
				$scope.MenuInfo = data.MenuInfo;
			});
			$cookies.put("type",data.AccountType);
			$cookies.put("LoginToken",data.LoginToken);
			$cookies.put("user",data.LoginName);
			$cookies.put("Company",data.Company);

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
app.controller('water1', function ($scope, $http, $cookies, $cookieStore, $rootScope) {
	var myChart1 = echarts.init(document.getElementById('one'));
	var option1 = {
		tooltip: {
			formatter: "{a} <br/>{b} : {c}%"
		},
		toolbox: {
			show: true,
			feature: {
				mark: {show: true},
				restore: {show: true},
				saveAsImage: {show: true}
			}
		},
		series: [
			{
				name: '水中寄生虫数',
				type: 'gauge',
				detail: {formatter: '{value}cell/L'},
				data: [{value: 0, name: '寄生虫数'}],
				min: 0,
				max: 5000,
				splitNumber: 10,
				axisLine: { // 坐标轴线
					lineStyle: { // 属性lineStyle控制线条样式
						color: [[0.1, '#32d113'],
							[0.2, '#ffff71'], [1, '#de1010']],
						width: 10
					}
				}
			}
		]
	};

	clearInterval(one.timeTicket);
	one.timeTicket = setInterval(function () {
		a = 5000 - (Math.random() * 5000).toFixed(0);
		Math.floor(a);
		option1.series[0].data[0].value = a;
		myChart1.setOption(option1, true);
	}, 2000);

	var myChart2 = echarts.init(document.getElementById('two'));
	var option2 = {
		tooltip: {
			formatter: "{a} <br/>{b} : {c}%"
		},
		toolbox: {
			show: true,
			feature: {
				mark: {show: true},
				restore: {show: true},
				saveAsImage: {show: true}
			}
		},
		series: [
			{
				name: '悬浮物',
				type: 'gauge',
				detail: {formatter: '{value}mg/L'},
				data: [{value: 4, name: '悬浮物'}],
				min: 0,
				max: 800,
				splitNumber: 8,
				axisLine: { // 坐标轴线
					lineStyle: { // 属性lineStyle控制线条样式
						color: [[0.1, '#32d113'],
							[0.3, '#ffff71'], [1, '#de1010']],
						width: 10
					}
				}
			}
		]
	};

	clearInterval(two.timeTicket);
	two.timeTicket = setInterval(function () {
		a = 800 - ((Math.random()) * 800 ).toFixed(0);
		Math.floor(a);
		option2.series[0].data[0].value = a;
		myChart2.setOption(option2, true);
	}, 1000);

	var myChart3 = echarts.init(document.getElementById('three'));
	var option3 = {
		tooltip: {
			formatter: "{a} <br/>{b} : {c}%"
		},
		toolbox: {
			show: true,
			feature: {
				mark: {show: true},
				restore: {show: true},
				saveAsImage: {show: true}
			}
		},
		series: [
			{
				name: '化学物质',
				type: 'gauge',
				detail: {formatter: '{value}mg/L'},
				data: [{value: 4, name: '化学物质'}],
				min: 0,
				max: 600,
				splitNumber: 6,
				axisLine: { // 坐标轴线
					lineStyle: { // 属性lineStyle控制线条样式
						color: [[0.05, '#32d113'],
							[0.1, '#ffff71'], [1, '#de1010']],
						width: 10
					}
				}
			}
		]
	};
	clearInterval(three.timeTicket);
	three.timeTicket = setInterval(function () {
		a = 800 - ((Math.random()) * 800 ).toFixed(0);
		Math.floor(a);
		option3.series[0].data[0].value = a;
		myChart3.setOption(option3, true);
	}, 1000);

	var myChart4 = echarts.init(document.getElementById('four'));
	var option4 = {
		tooltip: {
			formatter: "{a} <br/>{b} : {c}%"
		},
		toolbox: {
			show: true,
			feature: {
				mark: {show: true},
				restore: {show: true},
				saveAsImage: {show: true}
			}
		},
		series: [
			{
				name: '有机物',
				type: 'gauge',
				detail: {formatter: '{value}mg/L'},
				data: [{value: 4, name: '有机物'}],
				min: 0,
				max: 600,
				splitNumber: 6,
				axisLine: { // 坐标轴线
					lineStyle: { // 属性lineStyle控制线条样式
						color: [[0.2, '#32d113'],
							[0.4, '#ffff71'], [1, '#de1010']],
						width: 10
					}
				}
			}
		]
	};
	clearInterval(four.timeTicket);
	four.timeTicket = setInterval(function () {
		a = 600 - ((Math.random()) * 600 ).toFixed(0);
		Math.floor(a);
		option4.series[0].data[0].value = a;
		myChart4.setOption(option4, true);
	}, 1000);
	var dom = document.getElementById("five");
	var myChartz = echarts.init(dom);
	var app = {};
	option = null;
	function randomData() {
		now = new Date(+now + oneDay);
		value = value + Math.random() * 21 - 10;
		return {
			name: now.toString(),
			value: [
				[now.getFullYear(), now.getMonth() + 1, now.getDate()].join('-'),
				Math.round(value)
			]
		}
	}

	var data = [];
	var now = +new Date(1997, 9, 3);
	var oneDay = 24 * 3600 * 1000;
	var value = Math.random() * 1000;
	for (var i = 0; i < 1000; i++) {
		data.push(randomData());
	}

	optionz = {
		title: {
			text: '排放污水量总计'
		},
		tooltip: {
			trigger: 'axis',
			formatter: function (params) {
				params = params[0];
				var date = new Date(params.name);
				return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
			},
			axisPointer: {
				animation: false
			}
		},
		xAxis: {
			type: 'time',
			splitLine: {
				show: false
			}
		},
		yAxis: {
			type: 'value',
			boundaryGap: [0, '100%'],
			splitLine: {
				show: false
			}
		},
		series: [{
			name: '模拟数据',
			type: 'line',
			showSymbol: false,
			hoverAnimation: false,
			data: data
		}]
	};

	app.timeTicket = setInterval(function () {

		for (var i = 0; i < 5; i++) {
			data.shift();
			data.push(randomData());
		}

		myChartz.setOption({
			series: [{
				data: data
			}]
		});
	}, 1000);
	;
	if (optionz && typeof optionz === "object") {
		var startTime = +new Date();
		myChartz.setOption(optionz, true);
		var endTime = +new Date();
		var updateTime = endTime - startTime;
		console.log("Time used:", updateTime);
	}
	;

	var dom = document.getElementById("six");
	var myChartx = echarts.init(dom);
	var app = {};
	option = null;
	function randomData() {
		now = new Date(+now + oneDay);
		value = value + Math.random() * 21 - 10;
		return {
			name: now.toString(),
			value: [
				[now.getFullYear(), now.getMonth() + 1, now.getDate()].join('-'),
				Math.round(value)
			]
		}
	}

	var data = [];
	var now = +new Date(1997, 9, 3);
	var oneDay = 24 * 3600 * 1000;
	var value = Math.random() * 1000;
	for (var i = 0; i < 1000; i++) {
		data.push(randomData());
	}

	optionx = {
		title: {
			text: '排放污水量总计'
		},
		tooltip: {
			trigger: 'axis',
			formatter: function (params) {
				params = params[0];
				var date = new Date(params.name);
				return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
			},
			axisPointer: {
				animation: false
			}
		},
		xAxis: {
			type: 'time',
			splitLine: {
				show: false
			}
		},
		yAxis: {
			type: 'value',
			boundaryGap: [0, '100%'],
			splitLine: {
				show: false
			}
		},
		series: [{
			name: '模拟数据',
			type: 'line',
			showSymbol: false,
			hoverAnimation: false,
			data: data
		}]
	};

	app.timeTicket = setInterval(function () {

		for (var i = 0; i < 5; i++) {
			data.shift();
			data.push(randomData());
		}

		myChartx.setOption({
			series: [{
				data: data
			}]
		});
	}, 1000);
	;
	if (optionx && typeof optionx === "object") {
		var startTime = +new Date();
		myChartx.setOption(optionx, true);
		var endTime = +new Date();
		var updateTime = endTime - startTime;
		console.log("Time used:", updateTime);
	}
	;

	var dom = document.getElementById("seven");
	var myChartw = echarts.init(dom);
	var app = {};
	option = null;
	function randomData() {
		now = new Date(+now + oneDay);
		value = value + Math.random() * 21 - 10;
		return {
			name: now.toString(),
			value: [
				[now.getFullYear(), now.getMonth() + 1, now.getDate()].join('-'),
				Math.round(value)
			]
		}
	}

	var data = [];
	var now = +new Date(1997, 9, 3);
	var oneDay = 24 * 3600 * 1000;
	var value = Math.random() * 1000;
	for (var i = 0; i < 1000; i++) {
		data.push(randomData());
	}

	optionw = {
		title: {
			text: '排放污水量总计'
		},
		tooltip: {
			trigger: 'axis',
			formatter: function (params) {
				params = params[0];
				var date = new Date(params.name);
				return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
			},
			axisPointer: {
				animation: false
			}
		},
		xAxis: {
			type: 'time',
			splitLine: {
				show: false
			}
		},
		yAxis: {
			type: 'value',
			boundaryGap: [0, '100%'],
			splitLine: {
				show: false
			}
		},
		series: [{
			name: '模拟数据',
			type: 'line',
			showSymbol: false,
			hoverAnimation: false,
			data: data
		}]
	};

	app.timeTicket = setInterval(function () {

		for (var i = 0; i < 5; i++) {
			data.shift();
			data.push(randomData());
		}

		myChartw.setOption({
			series: [{
				data: data
			}]
		});
	}, 1000);
	;
	if (optionw && typeof optionw === "object") {
		var startTime = +new Date();
		myChartw.setOption(optionw, true);
		var endTime = +new Date();
		var updateTime = endTime - startTime;
		console.log("Time used:", updateTime);
	}
	

   

        // var dom = document.getElementById("five");
var dom = document.getElementById("five");
        var myChartz = echarts.init(dom);
        var app = {};
        option = null;function randomData() {
            now = new Date(+now + oneDay);
            value = value + Math.random() * 21 - 10;
            return {
                name: now.toString(),
                value: [
                    [now.getFullYear(), now.getMonth() + 1, now.getDate()].join('-'),
                    Math.round(value)
                ]
            }
        }

        var data = [];
        var now = +new Date(1997, 9, 3);
        var oneDay = 24 * 3600 * 1000;
        var value = Math.random() * 1000;
        for (var i = 0; i < 1000; i++) {
            data.push(randomData());
        }

        optionz = {
            title: {
                text: '排放污水量总计'
            },
            tooltip: {
                trigger: 'axis',
                formatter: function (params) {
                    params = params[0];
                    var date = new Date(params.name);
                    return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
                },
                axisPointer: {
                    animation: false
                }
            },
            xAxis: {
                type: 'time',
                splitLine: {
                    show: false
                }
            },
            yAxis: {
                type: 'value',
                boundaryGap: [0, '100%'],
                splitLine: {
                    show: false
                }
            },
            series: [{
                name: '模拟数据',
                type: 'line',
                showSymbol: false,
                hoverAnimation: false,
                data: data
            }]
        };

        app.timeTicket = setInterval(function () {

            for (var i = 0; i < 5; i++) {
                data.shift();
                data.push(randomData());
            }

            myChartz.setOption({
                series: [{
                    data: data
                }]
            });
        }, 1000);;
        if (optionz && typeof optionz === "object")
        {
            var startTime = +new Date();
            myChartz.setOption(optionz, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            console.log("Time used:", updateTime);
        };

var dom = document.getElementById("six");
        var myChartx = echarts.init(dom);
        var app = {};
        option = null;function randomData() {
            now = new Date(+now + oneDay);
            value = value + Math.random() * 21 - 10;
            return {
                name: now.toString(),
                value: [
                    [now.getFullYear(), now.getMonth() + 1, now.getDate()].join('-'),
                    Math.round(value)
                ]
            }
        }

        var data = [];
        var now = +new Date(1997, 9, 3);
        var oneDay = 24 * 3600 * 1000;
        var value = Math.random() * 1000;
        for (var i = 0; i < 1000; i++) {
            data.push(randomData());
        }

        optionx = {
            title: {
                text: '排放污水量总计'
            },
            tooltip: {
                trigger: 'axis',
                formatter: function (params) {
                    params = params[0];
                    var date = new Date(params.name);
                    return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
                },
                axisPointer: {
                    animation: false
                }
            },
            xAxis: {
                type: 'time',
                splitLine: {
                    show: false
                }
            },
            yAxis: {
                type: 'value',
                boundaryGap: [0, '100%'],
                splitLine: {
                    show: false
                }
            },
            series: [{
                name: '模拟数据',
                type: 'line',
                showSymbol: false,
                hoverAnimation: false,
                data: data
            }]
        };

        app.timeTicket = setInterval(function () {

            for (var i = 0; i < 5; i++) {
                data.shift();
                data.push(randomData());
            }

            myChartx.setOption({
                series: [{
                    data: data
                }]
            });
        }, 1000);;
        if (optionx && typeof optionx === "object")
        {
            var startTime = +new Date();
            myChartx.setOption(optionx, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            console.log("Time used:", updateTime);
        };

var dom = document.getElementById("seven");
        var myChartw = echarts.init(dom);
        var app = {};
        option = null;function randomData() {
            now = new Date(+now + oneDay);
            value = value + Math.random() * 21 - 10;
            return {
                name: now.toString(),
                value: [
                    [now.getFullYear(), now.getMonth() + 1, now.getDate()].join('-'),
                    Math.round(value)
                ]
            }
        }

        var data = [];
        var now = +new Date(1997, 9, 3);
        var oneDay = 24 * 3600 * 1000;
        var value = Math.random() * 1000;
        for (var i = 0; i < 1000; i++) {
            data.push(randomData());
        }

        optionw = {
            title: {
                text: '排放污水量总计'
            },
            tooltip: {
                trigger: 'axis',
                formatter: function (params) {
                    params = params[0];
                    var date = new Date(params.name);
                    return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
                },
                axisPointer: {
                    animation: false
                }
            },
            xAxis: {
                type: 'time',
                splitLine: {
                    show: false
                }
            },
            yAxis: {
                type: 'value',
                boundaryGap: [0, '100%'],
                splitLine: {
                    show: false
                }
            },
            series: [{
                name: '模拟数据',
                type: 'line',
                showSymbol: false,
                hoverAnimation: false,
                data: data
            }]
        };

        app.timeTicket = setInterval(function () {

            for (var i = 0; i < 5; i++) {
                data.shift();
                data.push(randomData());
            }

            myChartw.setOption({
                series: [{
                    data: data
                }]
            });
        }, 1000);;
        if (optionw && typeof optionw === "object")
        {
            var startTime = +new Date();
            myChartw.setOption(optionw, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            console.log("Time used:", updateTime);
        }
	$scope.paginationConf = {
		totalItems: 20,
		currentPage: 1,
		itemsPerPage: 10,
		pagesLength: 15,
		perPageOptions: [10, 20, 30, 40, 50],
		onChange: function () {
			var Show1 = $.ajax({
				type: 'GET',
				url: $scope.HttpUrl + 'WaterMonitor' + '?token=' + token,
				data: {
					currentPage: $scope.paginationConf.currentPage,
					itemsPerPage: $scope.paginationConf.itemsPerPage,

				},
				dataType: 'json',
			});
			Show1.success(function (data, status, headers, config) {
				$scope.$apply(function () {
					$scope.water1Data = data.model;
					$scope.paginationConf.totalItems = data.Count;
				});
				console.log(data);
			});
			Show1.error(function (responese, status, headers, config) {
				alert(responese.responseJSON);
				location.href = "login.html";
				console.log(responese.responseJSON);
				if (responese.status == 401) {
					alert:"用户验证已过期";
					location.href = "login.html"
				}
				console.log(status);
				console.log(headers);
				console.log(config);
			});
		}

	};


	$scope.water1 = {
		MonitorNumber: "",
		Pressure: "",
		FlowRate: "",
		TotalRate: "",
		PSM: "",
		SupplyVoltage: ""
	};
	var token = $scope.token;
	if (token == null) {
		alert("请先登录！");
		location.href = "login.html";
	}
	else {
		$scope.showwater1 = function (getwater1) {
			var Show = $.ajax({
				type: 'GET',
				url: $scope.HttpUrl + 'WaterMonitor' + '?token=' + token,
				data: {},
				dataType: 'json'
			});
			Show.success(function (data, status, config, headers) {
				$scope.$apply(function () {
					$scope.water1Data = data;
				});
			});
			Show.error(function (response, headers, status, config) {
				if (responese.status == 401) {
					alert:"用户验证已过期";
					location.href = "login.html"
				}
				else {
					alert:"error";
				}
			})
		};
	}
	;
	var token = $scope.token;
	if (token == null) {
		alert("请先登录！");
		location.href = "login.html";
	}
	else {
		$scope.Show = function (getUser) {                         //查
			var Show1 = $.ajax({
				type: 'GET',
				url: $scope.HttpUrl + 'WaterMonitor' + '?token=' + token,
				data: {},
				dataType: 'json',
			});
			Show1.success(function (data, status, headers, config) {
				$scope.$apply(function () {
					$scope.water1Data = data;

				});
				console.log(data);
			});
			Show1.error(function (responese, status, headers, config) {
				alert(responese.responseJSON);
				location.href = "login.html";
				console.log(responese.responseJSON);
				if (responese.status == 401) {
					alert:"用户验证已过期";
					location.href = "login.html"
				}
				console.log(status);
				console.log(headers);
				console.log(config);
			});
		};
	};

	$scope.Insertwater1 = function (water1) {
		var Insert = $.ajax({
			type: 'POST',
			url: $scope.HttpUrl + 'WaterMonitor' + '?token=' + token,
			headers: {
				'Content-Type': 'application/x-www-form-urlencoded'
			},
			data: {
				MonitorNumber: water1.MonitorNumber,
				Pressure: water1.Pressure,
				FlowRate: water1.FlowRate,
				TotalRate: water1.TotalRate,
				PSM: water1.PSM,
				SupplyVoltage: water1.SupplyVoltage


			},
			dataType: 'json'
		});
		Insert.success(function (data, config, headers, status) {
			console.log($scope.data);
			window.location.reload(true);
		});
		Insert.error(function (response, config, headers, status) {
			alert("error");
		});
	};

	$scope.Deletewater1 = function (x) {                      //删
		var ids = "";
		var flag;
		$("input:checkbox:checked").each(function () {
			flag = true;
			ids += $(this).val() + ",";
		})
		if (!flag) {
			alert("请至少选择一项");
		}
		if (ids.length > 0)
			if (confirm("是否删除所选条目？")) {
				ids = ids.substring(0, ids.length - 1);
				var Delete = $.ajax({
					type: 'POST',
					url: $scope.HttpUrl + "WaterMonitor" + "?token=" + token,
					data: {
						"Remark": ids,
						"HttpMethod": "DELETE",
					},
					dataType: 'json',
				});
				Delete.success(function (data, status, headers, config) {
					alert("删除成功");
					//show(getShow);
					window.location.reload(true);
				});
				Delete.error(function (responese, status, headers, config) {
					console.log(responese);
					alert("操作失败");
				});
			}
		;
	}
});	
app.controller('water2', function ($scope) {
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
app.controller('water3', function ($scope) {
	var token = $scope.token;
	var Show1 = $.ajax({
		type: 'POST',
		url: $scope.HttpUrl + 'Analysis' + '?token=' + token,
		data: {
			Year: '2016',
			Type: 'WASTEWATER',
		},
		dataType: 'json'
	});
		Show1.success(function (data, status, headers, config) {
			var myChart = echarts.init(document.getElementById('one'));
		option = {
			title: {
				text: '近一年污水超标次数',
				subtext: '河北衡水'
			},
			tooltip: {
				trigger: 'axis'
			},
			legend: {
				data: ['污水超标次数']
			},
			toolbox: {
				show: true,
				feature: {
					dataView: {show: true, readOnly: false},
					magicType: {show: true, type: ['line', 'bar']},
					restore: {show: true},
					saveAsImage: {show: true}
				}
			},
			calculable: true,
			xAxis: [
				{
					type: 'category',
					data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
				}
			],
			yAxis: [
				{
					type: 'value'
				}
			],
			series: [
				{
					name: '污水超标次数',
					type: 'bar',
					//data: [2.0, 4.9, 7.0, 23.2, 25.6, 76.7, 135.6, 162.2, 32.6, 20.0, 6.4, 3.3],
					data: data,
					markPoint: {
						data: [
							{type: 'max', name: '最大值'},
							{type: 'min', name: '最小值'}
						]
					},
					markLine: {
						data: [
							{type: 'average', name: '平均值'}
						]
					}
				}
			]
		};
		myChart.setOption(option, true);
	});
		Show1.error(function (responese, status, headers, config) {
			alert(responese.responseJSON);
			//location.href = "login.html";
			console.log(responese.responseJSON);
			if (responese.status == 401) {
				alert:"用户验证已过期";
				location.href = "login.html"
			}
			console.log(status);
			console.log(headers);
			console.log(config);
		});




	var myChart1 = echarts.init(document.getElementById('two'));
	option1 = {
		title : {
			text: '超标最多的企业',
			subtext: '纯属虚构',
			x:'center'
		},
		tooltip : {
			trigger: 'item',
			formatter: "{a} <br/>{b} : {c} ({d}%)"
		},
		legend: {
			orient: 'vertical',
			left: 'left',
			data: ['蓝天化工','冀衡化肥','东北化工','衡林生物','冠龙农化',"冀衡药业"]
		},
		series : [
			{
				name: '访问来源',
				type: 'pie',
				radius : '55%',
				center: ['50%', '60%'],
				data:[
					{value:32, name:'蓝天化工'},
					{value:31, name:'冀衡化肥'},
					{value:23, name:'东北化工'},
					{value:22, name:'衡林生物'},
					{value:16, name:'冠龙农化'},
					{value:14, name:'冀衡药业'}
				],
				itemStyle: {
					emphasis: {
						shadowBlur: 10,
						shadowOffsetX: 0,
						shadowColor: 'rgba(0, 0, 0, 0.5)'
					}
				}
			}
		]
	};
	myChart1.setOption(option1, true);


	var myChart2 = echarts.init(document.getElementById('three'));
	option2 = {
		tooltip : {
			trigger: 'axis',
			axisPointer : {            // 坐标轴指示器，坐标轴触发有效
				type : 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
			}
		},
		legend: {
			data:['悬浮物(毫克/升)','病原体(万个/升)','需氧有机物(毫克/升)','植物营养素(毫克/升)','无机污染物(毫克/升)']
		},
		grid: {
			left: '3%',
			right: '4%',
			bottom: '3%',
			containLabel: true
		},
		xAxis : [
			{
				type : 'category',
					data : ['A','B','C','D','E']
			}
		],
		yAxis : [
			{
				type : 'value'
			}
		],
		series : [
			{
				name:'悬浮物(毫克/升)',
				type:'bar',
				data:[212, 376, 211,456, 875]
			},
			{
				name:'病原体(万个/升)',
				type:'bar',
				stack: '广告',
				data:[52, 73, 1200, 5047, 685]
			},
			{
				name:'需氧有机物(毫克/升)',
				type:'bar',
				stack: '广告',
				data:[520, 1182, 191, 434, 590]
			},
			{
				name:'植物营养素(毫克/升)',
				type:'bar',
				stack: '广告',
				data:[1350, 232, 2001, 154, 190]
			},
			{
				name:'无机污染物(毫克/升)',
				type:'bar',
				data:[862, 1018, 964, 1026, 1670],
				markLine : {
					lineStyle: {
						normal: {
							type: 'dashed'
						}
					},
					data : [
						[{type : 'min'}, {type : 'max'}]
					]
				}
			},
		]
	};
	myChart2.setOption(option2, true);





	var myChart3 = echarts.init(document.getElementById('four'));
	var option3 =
	{
		title : {
			text: '过去一周污染物变化',
			subtext: '河北衡水'
		},
		tooltip : {
			trigger: 'axis'
		},
		legend: {
			data:['悬浮物','病原体']
		},
		toolbox: {
			show : false,
			feature : {
				dataZoom: {},
				dataView: {readOnly: false},
				magicType: {type: ['line', 'bar']},
				restore: {},
				saveAsImage: {}
			}
		},
		xAxis : [
			{
				type : 'category',
				boundaryGap : false,
				data : ['周一','周二','周三','周四','周五','周六','周日']
			}
		],
		yAxis : [
			{
				type : 'value',
				max : 'dataMax',
				min : '0',
				axisLabel : {
					formatter: '{value} '
				}
			}
		],
		series : [
			{
				name:'悬浮物浓度',
				type:'line',
				data:[11, 11, 15, 13, 12, 13, 10],

				markLine : {
					data : [
						{type : 'average', name: '平均值'}
					]
				}
			},
			{
				name:'病原体数量',
				type:'line',
				data:[1, 2, 2, 5, 3, 2, 5],
				markLine : {
					data : [
						{type : 'average', name : '平均值'}
					]
				}
			}
		]
	};
	myChart3.setOption(option3, true);


	var myChart4 = echarts.init(document.getElementById('five'));
	var option4= {
		title : {
			text: '污染物比例',
			subtext: '   ',
			x:'center'
		},
		tooltip : {
			trigger: 'item',
			formatter: "{a} <br/>{b} : {c} ({d}%)"
		},
		legend: {
			orient: 'vertical',
			left: 'left',
			data: ['酸','碱','氧化剂','铜','镉']
			// ,'汞','苯'
		},
		series : [
			{
				name: '污染物',
				type: 'pie',
				radius : '55%',
				center: ['50%', '60%'],
				data:[
					{value:335, name:'酸'},
					{value:310, name:'碱'},
					{value:234, name:'氧化剂'},
					{value:135, name:'铜'},
					{value:561, name:'镉'}
					// {value:628, name:'汞'}
					// {value:1548, name:'苯'}
				],
				itemStyle: {
					emphasis: {
						shadowBlur: 10,
						shadowOffsetX: 0,
						shadowColor: 'rgba(0, 0, 0, 0.5)'
					}
				}
			}
		]
	};
	myChart4.setOption(option4, true);
	var myChart = echarts.init(document.getElementById('one'));
var option = {
    title : {
        text: '污染物比例',
        subtext: '   ',
        x:'center'
    },
    tooltip : {
        trigger: 'item',
        formatter: "{a} <br/>{b} : {c} ({d}%)"
    },
    legend: {
        orient: 'vertical',
        left: 'left',
        data: ['酸','碱','氧化剂','铜','镉']
        // ,'汞','苯'
    },
    series : [
        {
            name: '污染物',
            type: 'pie',
            radius : '55%',
            center: ['50%', '60%'],
            data:[
                {value:335, name:'酸'},
                {value:310, name:'碱'},
                {value:234, name:'氧化剂'},
                {value:135, name:'铜'},
                {value:561, name:'镉'}
                // {value:628, name:'汞'}
                // {value:1548, name:'苯'}
            ],
            itemStyle: {
                emphasis: {
                    shadowBlur: 10,
                    shadowOffsetX: 0,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            }
        }
    ]
};
myChart.setOption(option, true);

var myChart2 = echarts.init(document.getElementById('two'));
var option2 = 
{
    title : {
            text: '过去一周污染物变化',
            subtext: '河北衡水'
        },
        tooltip : {
            trigger: 'axis'
        },
        legend: {
            data:['悬浮物','病原体']
        },
        toolbox: {
            show : false,
            feature : {
                dataZoom: {},
                dataView: {readOnly: false},
                magicType: {type: ['line', 'bar']},
                restore: {},
                saveAsImage: {}
            }
        },
        xAxis : [
            {
                type : 'category',
                boundaryGap : false,
                data : ['周一','周二','周三','周四','周五','周六','周日']
            }
        ],
        yAxis : [
            {
                type : 'value',
                max : 'dataMax',
                min : '0',
                axisLabel : {
                    formatter: '{value} '
                }
            }
        ],
        series : [
            {
                name:'悬浮物浓度',
                type:'line',
                data:[11, 11, 15, 13, 12, 13, 10],

                markLine : {
                    data : [
                        {type : 'average', name: '平均值'}
                    ]
                }
            },
            {
                name:'病原体数量',
                type:'line',
                data:[1, 2, 2, 5, 3, 2, 5],
                markLine : {
                    data : [
                        {type : 'average', name : '平均值'}
                    ]
                }
            }
        ]
        };       
myChart2.setOption(option2, true);
});

app.controller('water4', function ($scope) {
	var token = $scope.token;
	var Show1 = $.ajax({
		type: 'POST',
		url: $scope.HttpUrl + 'Analysis' + '?token=' + token,
		data: {
			Year: '2016',
			Type: 'WASTEWATER',
		},
		dataType: 'json'
	});
	Show1.success(function (data, status, headers, config) {
		var myChart = echarts.init(document.getElementById('one'));
		option = {
			title : {
				text: '统计分析',
				subtext: '纯属虚构'
			},
			tooltip : {
				trigger: 'axis'
			},
			legend: {
				data:['预计处理污水量','实际处理污水量']
			},
			toolbox: {
				show : true,
				feature : {
					dataView : {show: true, readOnly: false},
					magicType : {show: true, type: ['line', 'bar']},
					restore : {show: true},
					saveAsImage : {show: true}
				}
			},
			calculable : true,
			xAxis : [
				{
					type : 'category',
					data : ['A处理厂','B处理厂','C处理厂','D处理厂','E处理厂']
				}
			],
			yAxis : [
				{
					type : 'value'
				}
			],
			series : [
				{
					name:'预计处理污水量',
					type:'bar',
					data:[15, 12, 15, 33, 14],
					markPoint : {
						data : [
							{type : 'max', name: '最大值'},
							{type : 'min', name: '最小值'}
						]
					},
					markLine : {
						data : [
							{type : 'average', name: '平均值'}
						]
					}
				},
				{
					name:'实际处理污水量',
					type:'bar',
					data:[16, 10, 18, 17, 15],
				}
			]
		};
		myChart.setOption(option, true);
	});
	Show1.error(function (responese, status, headers, config) {
		alert(responese.responseJSON);
		location.href = "login.html";
		console.log(responese.responseJSON);
		if (responese.status == 401) {
			alert:"用户验证已过期";
			location.href = "login.html"
		}
		console.log(status);
		console.log(headers);
		console.log(config);
	});


});
app.controller('UserManagement', function ($scope, $http, $cookies, $cookieStore, $rootScope) {
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
					itemsPerPage:$scope.paginationConf.itemsPerPage

				},
				dataType:'json'
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
		Email:"",
		AccountType:"",
		Company:"",
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
				dataType:'json'
			});
			Show1.success(function(data,status,headers,config){
				$scope.$apply(function(){
					$scope.User = data;
				});
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
			var Show2 = $.ajax({
				type:'GET',
				url:$scope.HttpUrl +'company'+'?token='+token,
				data:{
				},
				dataType:'json'
			});
			Show2.success(function(data,status,headers,config){
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
		};
	};

	$scope.InsertUser = function(User){                      //增
		var name = "";
		if(User.Company == null)
		{
			name = "";
		}
		else
		{
			name = User.Company.CompanyName;
		};
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
				Company: name,
				HttpMethod:'POST'
			},
			dataType:'json'
		});
		InsertUser1.success(function(data,status,headers,config){
			alert("保存成功！");
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
        		        "HttpMethod":"DELETE"
        		    },
        		    dataType: 'json'
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
        	}
    };
});



