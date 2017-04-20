var myChartindexone = echarts.init(document.getElementById('indexone'));
        var optionindexone = {
            title : {
            text: '上周环保数据变化',
            subtext: '河北衡水'
        },
        tooltip : {
            trigger: 'axis'
        },
        legend: {
            data:['水质信息','大气质量']
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
                axisLabel : {
                    formatter: '{value} '
                }
            }
        ],
        series : [
            {
                name:'水质信息',
                type:'line',
                data:[11, 11, 15, 13, 12, 13, 10],
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
                name:'大气质量',
                type:'line',
                data:[1, 0, 2, 5, 3, 2, 0],
                markPoint : {
                    data : [
                        {name : '周最低', value : 0, xAxis: 1, yAxis: 0}
                    ]
                },
                markLine : {
                    data : [
                        {type : 'average', name : '平均值'}
                    ]
                }
            }
        ]
        };
        myChartindexone.setOption(optionindexone);

var myChartindextwo = echarts.init(document.getElementById('indextwo'));
        var optionindextwo = {
            title : {
            text: '污染雷达图',
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
        myChartindextwo.setOption(optionindextwo);

var myChartindexthree = echarts.init(document.getElementById('indexthree'));
        var optionindexthree = {
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
        myChartindexthree.setOption(optionindexthree);