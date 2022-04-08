import moment from "moment";

export class MyFilters {
    public static formatDate(value: string): string {
        return moment(value).format("YYYY-MM-DD hh:mm:ss");
    }
}
