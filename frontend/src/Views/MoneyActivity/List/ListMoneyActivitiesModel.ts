import { Vue, Component } from "vue-property-decorator";
import { IDataTableItem } from "../../../Common/DataTable/DataTableModel";
import { ActivityType, PaymentType } from "../../../Common/Enums";
import ConfirmationDialogModel from "../../../Common/Components/ConfirmationDialog/ConfirmationDialogModel";

@Component({
    name: "ListMoneyActivities"
})
export default class ListMoneyActivities extends Vue {
    public items: IListMoneyActivityItem[] = null!;
    public search: string = "";
    public headers: IDataTableItem[] = [
        {
            text: "",
            value: "action",
            sortable: false,
            align: "left"
        },
        {
            text: "Amount",
            align: "left",
            sortable: true,
            value: "amount"
        },
        {
            text: "Cash activity date",
            align: "left",
            sortable: true,
            value: "cashActivityDate"
        },
        {
            text: "Category",
            align: "left",
            sortable: true,
            value: "categoryName"
        },
        {
            text: "Business",
            align: "left",
            sortable: true,
            value: "businessName"
        },
        {
            text: "Payment type",
            align: "left",
            sortable: true,
            value: "paymentType"
        },
        {
            text: "Activity type",
            align: "left",
            sortable: true,
            value: "activityType"
        }
    ];

    public isIncomeActivityType(value: ActivityType): boolean {
        return ActivityType.Income === value;
    }

    public isSavingActivityType(value: ActivityType): boolean {
        return ActivityType.Saving === value;
    }

    public isExpenditureActivityType(value: ActivityType): boolean {
        return ActivityType.Expenditure === value;
    }

    public getCashActivityDate(cashActivityDate: Date): string {
        return new Date(cashActivityDate.toString()).toISOString().substr(0, 10);
    }

    public getActivityTypeAsString(value: ActivityType): string {
        switch (value) {
            case ActivityType.Income:
                return "Income";
            case ActivityType.Saving:
                return "Saving";
            case ActivityType.Expenditure:
                return "Expenditure";
        }
    }

    public getPaymentTypeAsString(value: PaymentType): string {
        switch (value) {
            case PaymentType.Cash:
                return "Cash";
            case PaymentType.CreditCard:
                return "CreditCard";
            case PaymentType.GiroCard:
                return "GiroCard";
            case PaymentType.Paypal:
                return "Paypal";
        }
    }

    public createMoneyActivity(): void {
        this.$router.push({ name: "CreateOrEditMoneyActivity" });
    }

    public editMoneyActivity(item: IListMoneyActivityItem): void {
        this.$router.push({ name: "CreateOrEditMoneyActivity", params: { id: item.id } });
    }

    public showStatistic(): void {
        this.$router.push({ name: "VisualizeMoneyActivity" });
    }

    public async deleteMoneyActivity(item: IListMoneyActivityItem): Promise<void> {
        const confirmationDialog = this.$refs.confirmationDialog as ConfirmationDialogModel;
        const dialogResult = await confirmationDialog.open("Delete", "Are you sure?");
        if (!dialogResult) {
            return;
        }

        await this.$http.postSingleRequestAsync("moneyactivity", "delete", { id: item.id });
        const index = this.items.indexOf(item);
        this.items.splice(index, 1);
    }

    public async mounted(): Promise<void> {
        this.items = await this.prepareModelAsync();
    }

    private async prepareModelAsync(): Promise<IListMoneyActivityItem[]> {
        const items = await this.$http.getAsync<IListMoneyActivityItem[]>("moneyactivity", "list");
        const model = items.sort((x, y) => x.cashActivityDate < y.cashActivityDate ? 1 : -1);
        return model;
    }
}

interface IListMoneyActivityItem {
    id: string;
    amount: string;
    categoryName: string;
    businessName: string;
    cashActivityDate: Date;
}
