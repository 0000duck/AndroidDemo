function bindLine(dataList, title, xList, typedate) {
    var testLine = echarts.init(document.getElementById("lineBox"));
    $("#pieBox").hide();
    testLine.showLoading({
        text: '正在努力的读取数据中...'
    });
    var option = setOptionLine(dataList, title, xList, typedate);
    testLine.hideLoading();
    testLine.setOption(option);
}
function setOptionLine(data, title, xList, typedate) {
    var option = {                                                      
        title: {
            text: title,                        
        },
        tooltip: {                                                  
            trigger: 'axis'                                         
        },
        toolbox: {                                                  
            show: true,                                            
            feature: {                                              
                magicType: { show: true, type: ['bar', 'line'] },   
                restore: { show: true },                           
            }
        },
        calculable: true,                                          
        xAxis: [                                                   
            {
                type: 'category',
                boundaryGap: true,                                 
                data: xList    
            }
        ],
        yAxis: [                                                   
            {
                type: 'value',
                axisLabel: {                                        
                    formatter: '{value} 人次'                        
                }
            }
        ],
        series: [
            {
                name: '最多人次',                                   
                type: typedate,                                       
                data: data,                 
                markPoint: {                                       
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                },
                markLine: {                                         
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                }
            }
        ]
    };
    return option;
}
function bindPie(dataList) {
    var testPie = echarts.init(document.getElementById("pieBox"));
    $("#lineBox").hide();

    testPie.showLoading({
        text: '正在努力的读取数据中...'
    });
    var option = setOptionPie(dataList);
    testPie.hideLoading();
    testPie.setOption(option);
}
function setOptionPie(data) {
    var legend_data = [];
    if (data && data.length > 0) {
        $.each(data, function (idx, d) {
            legend_data.push(d.name);
        });
    }
    var option = {
        title: {
            text: data.title || '',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        toolbox: {
            show: true,
            feature: {
                restore: { show: true }
            }
        },
        legend: {
            orient: 'vertical',
            x: 'left',
            data: legend_data
        },
        calculable: true,
        series: [
            {
                type: 'pie',
                radius: '50%',
                center: ['50%', '50%'],
                data: data,
                itemStyle: {
                    normal: {
                        label: {
                            show: true,
                            position: 'outer',
                            formatter: "{b}\n{d}%",
                        },
                        labelLine: {
                            show: true,
                        }
                    },
                    emphasis: {
                        label: {
                            show: true,
                            formatter: "{d}%"
                        }
                    }

                }
            }
        ]
    };
    return option;
}