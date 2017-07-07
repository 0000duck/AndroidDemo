package wyq.gy.Bean;

/**
 * Created by wangcheng on 2017/3/9.
 */

public class historyOrder {
    private String title;
    private String state;
    private String date;
    private String address;
    private String workContent;

    public historyOrder() {

    }

    public historyOrder(String title, String state, String date, String address, String workContent) {
        this.title = title;
        this.state = state;
        this.date=date;
        this.address=address;
        this.workContent=workContent;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getState() {
        return state;
    }

    public void setState(String state) {
        this.state = state;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getworkContent() {
        return workContent;
    }

    public void setworkContent(String workContent) {
        this.workContent = workContent;
    }

    @Override
    public String toString() {
        return "datas{" +
                "title='" + title + '\'' +
                ", state='" + state + '\'' +
                ", date='" + date + '\'' +
                ", address='" + address + '\'' +
                ", workContent='" + workContent + '\'' +
                '}';
    }









}
