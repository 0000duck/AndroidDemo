
namespace NetSchool.Common.Info
{
    /// <summary>
    /// PageInfo 的摘要说明。
    ///  分页查询中的参数，用来传递分页信息
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageIndex;
        /// <summary>
        /// 记录数
        /// </summary>
        public int RecordCount;
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount;
        /// <summary>
        /// 最后一页记录数
        /// </summary>
        public int RecordsInLastPage;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize;
        /// <summary>
        /// 是否分页：false表示仅按照所给的字段进行排序操作
        /// </summary>
        public bool IsPage;

        /// <summary>
        /// <para>是否取前几行：</para>
        /// <para>仅当IsPage为false时生效，</para>
        /// <para>取出当前排序的前几个数据，</para>
        /// <para>数目由PageSize决定</para>
        /// </summary>
        public bool IsSelectTop;

        /// <summary>
        /// 第一个排序字段
        /// </summary>
        public string SortField1;
        /// <summary>
        /// 第一个排序字段排序方法
        /// </summary>
        public Enums.SortType SortType1;
        /// <summary>
        /// 第二个排序字段
        /// </summary>
        public string SortField2;
        /// <summary>
        /// 第二个排序字段排序方法
        /// </summary>
        public Enums.SortType SortType2;
        /// <summary>
        /// 第三个排序字段
        /// </summary>
        public string SortField3;
        /// <summary>
        /// 第三个排序字段排序方法
        /// </summary>
        public Enums.SortType SortType3;

        public PageInfo()
        {
            IsSelectTop = false;
            IsPage = true;
            CurrentPageIndex = 0;
            PageSize = 10;
            RecordCount = 0;
            SortField1 = null;
            SortType1 = Enums.SortType.ASC;
            SortField2 = null;
            SortType2 = Enums.SortType.ASC;
            SortField3 = null;
            SortType3 = Enums.SortType.ASC;
        }
    }
}
