var hebeiFilters = angular.module('hebeiFilters', []);
hebeiFilters.filter("pushsetting",function(){
	return function(input){
		var out = "";
		if (input == 1) 
			{
				out = "短信推送";
			};
		if (input == 2) 
			{
				out = "水推送"
			};
		if (input == 3) 
			{
				out = "废气推送"
			};
		if (input == 4) 
			{
				out = "预警决策"
			};
		if (input == 5) 
			{
				out = "设备"
			};
		if (input == 6) 
			{
				out = "报警信息"
			};
		return out;
	}
});
hebeiFilters.filter("pushtype",function(){
	return function(input){
		var out = "";
		if (input == 1)
		{
			out = "数据信息"
		};
		if (input == 2)
		{
			out = "数据异常"
		};
		if (input == 3)
		{
			out = "警告信息"
		};

		if (input == 4)
		{
			out = "异常报警"
		};
		return out;
	}
});
hebeiFilters.filter("accounttype",function(){
	return function(input){
		var out = "";
		if (input == 1) 
			{
				out = "管理员"
			};
		if (input == 2) 
			{
				out = "园区领导"
			};
		if (input == 3) 
			{
				out = "企业领导"
			};
		if (input == 4) 
			{
				out = "园区管理员"
			};
		if (input == 5) 
			{
				out = "企业一般管理员"
			};
		return out;
	}
});
hebeiFilters.filter("sex",function(){
	return function(input){
		var out = "";
		if (input == 1) 
			{
				out = "男"
			};
		if (input == 2)
			{
				out = "女"
			};
		return out;
	}
});
hebeiFilters.filter("warningType",function(){
	return function(input){
		var out = "";
		if (input == 0)
		{
			out = "未审核"
		};
		if (input == 1)
		{
			out = "审核通过"
		};
		if (input == 2)
		{
			out = "审核未通过"
		};
		return out;
	}
});
hebeiFilters.filter("equipmentstatus",function(){
	return function(input){
		var out = "";
		if (input == 0)
		{
			out = "正常"
		};
		if (input == 1)
		{
			out = "异常"
		};
		return out;
	}
});
hebeiFilters.filter("result",function(){
	return function(input){
		var out = "";
		if (input == 0)
		{
			out = "完成"
		};
		if (input == 1)
		{
			out = "未完成"
		};
		return out;
	}
});
hebeiFilters.filter("null",function(){
	return function(input){
		var out="";
		if (input == null)
		{
			out = "无"
		}
		else
		{
			out = input
		};
		return out;
	}
});
hebeiFilters.filter("pollution",function(){
	return function(input){
		var out ="";
		if (input == 0)
		{
			out = "SO2"
		};
		if (input == 1)
		{
			out = "酸碱度"
		};
		if (input == 2)
		{
			out = "锌"
		};
		if (input == 3)
		{
			out = "镉"
		};
		if (input == 4)
		{
			out = "汞"
		};
		return out;
	}
});
hebeiFilters.filter("letgotype",function(){
	return function(input){
		var out ="";
		if (input == 0)
		{
			out = "城市下水道"
		};
		if (input == 1)
		{
			out = "污水厂"
		};
		if (input == 2)
		{
			out = "其他单位"
		};
		return out;
	}
});
hebeiFilters.filter("overproof",function(){
	return function(input){
		var out ="";
		if (input == 0)
		{
			out = "正常"
		};
		if (input == 1)
		{
			out = "超标"
		};
		return out;
	}
});
hebeiFilters.filter("gastype",function(){
	return function(input){
		var out ="";
		if (input == 0)
		{
			out = "PM2.5"
		};
		if (input == 1)
		{
			out = "二氧化碳"
		};
		if (input == 2)
		{
			out = "二氧化硫"
		};
		if (input == 3)
		{
			out = "臭氧"
		};
		if (input == 4)
		{
			out = "氮氧化物"
		};
		return out;
	}
});
hebeiFilters.filter("letgotypegas",function(){
	return function(input){
		var out ="";
		if (input == 0)
		{
			out = "废气厂"
		};
		if (input == 1)
		{
			out = "大气"
		};
		if (input == 2)
		{
			out = "其他单位"
		};
		return out;
	}
});
hebeiFilters.filter("soildtype",function(){
	return function(input){
		var out ="";
		if (input == 0)
		{
			out = "热处理含氰废物"
		};
		if (input == 1)
		{
			out = "废乳 化液"
		};
		if (input == 2)
		{
			out = "废矿物油"
		};
		if (input == 3)
		{
			out = "含多氯联苯废物"
		};
		return out;
	}
});
hebeiFilters.filter("letgotypesoild",function(){
	return function(input){
		var out ="";
		if (input == 0)
		{
			out = "固废处理厂"
		};
		if (input == 1)
		{
			out = "废弃场"
		};
		if (input == 2)
		{
			out = "其他单位"
		};
		return out;
	}
});