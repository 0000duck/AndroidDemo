package hb.smartgreen.db;

import org.xutils.db.annotation.Column;
import org.xutils.db.annotation.Table;

import java.util.Date;

@Table(name = "NewsItem")
public class NewsItem {

//    @DatabaseField(generatedId = true)
    @Column(name = "id")
    private int id;    //ID

    @Column(name = "type")
    private int type;
    @Column(name = "title")
    private String title;
    @Column(name = "url")
    private String url;
    @Column(name = "date")
    private String date;
    @Column(name = "content")
    private String content;
    @Column(name = "pageNumber")
    private int pageNumber;
    @Column(name = "source")
    private String source;
    @Column(name = "updateTime")
    private Date updateTime;

    public Date getUpdateTime() {
        return updateTime;
    }

    public void setUpdateTime(Date updateTime) {
        this.updateTime = updateTime;
    }


    public String getSource() {
        return source;
    }

    public void setSource(String source) {
        this.source = source;
    }

    public int getPageNumber() {
        return pageNumber;
    }

    public void setPageNumber(int pageNumber) {
        this.pageNumber = pageNumber;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
        this.type = type;
    }


    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    @Override
    public String toString() {
        return "NewsItem{" +
                "id=" + id +
                ", title='" + title + '\'' +
                ", url='" + url + '\'' +
                ", date='" + date + '\'' +
                ", content='" + content + '\'' +
                '}';
    }



}
