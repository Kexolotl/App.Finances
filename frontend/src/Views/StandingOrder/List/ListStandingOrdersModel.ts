import { Vue, Component } from "vue-property-decorator";
import { IDataTableItem } from "../../../Common/DataTable/DataTableModel";
import ConfirmationDialogModel from "../../../Common/Components/ConfirmationDialog/ConfirmationDialogModel";

@Component({
    name: "ListStandingOrders"
})
export default class ListStandingOrders extends Vue {
    public items: IListStandingOrderItem[] = null!;
    public search: string = "";
    public headers: IDataTableItem[] = [
        {
            text: "",
            value: "action",
            sortable: false,
            align: "left"
        },
        {
            text: "Is active",
            align: "left",
            sortable: false,
            value: "isActive"
        },
        {
            text: "Amount",
            align: "left",
            sortable: true,
            value: "amount"
        },
        {
            text: "First payment date",
            align: "left",
            sortable: true,
            value: "firstPaymentDate"
        },
        {
            text: "Final payment date",
            align: "left",
            sortable: true,
            value: "finalPaymentDate"
        },
        {
            text: "Next payment date",
            align: "left",
            sortable: true,
            value: "nextPaymentDate"
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
        }
    ];

    public getFormattedDate(date: Date): string | null {
        return new Date(date.toString()).toISOString().substr(0, 10);
    }

    public createStandingOrder(): void {
        this.$router.push({ name: "CreateOrEditStandingOrder" });
    }

    public editStandingOrder(item: IListStandingOrderItem): void {
        this.$router.push({ name: "CreateOrEditStandingOrder", params: { id: item.id } });
    }

    public async deleteStandingOrder(item: IListStandingOrderItem): Promise<void> {
        const confirmationDialog = this.$refs.confirmationDialog as ConfirmationDialogModel;
        const dialogResult = await confirmationDialog.open("Delete", "Are you sure?");
        if (!dialogResult) {
            return;
        }

        await this.$http.postSingleRequestAsync("StandingOrder", "Delete", { id: item.id });
        const index = this.items.indexOf(item);
        this.items.splice(index, 1);
    }

    public async mounted(): Promise<void> {
        this.items = await this.prepareModelAsync();
    }

    private async prepareModelAsync(): Promise<IListStandingOrderItem[]> {
        const items = await this.$http.getAsync<IListStandingOrderItem[]>("StandingOrder", "List");
        const model = items.sort((x, y) => x.firstPaymentDate < y.firstPaymentDate ? 1 : -1);
        return model;
    }
}

interface IListStandingOrderItem {
    id: string;
    amount: string;
    categoryName: string;
    businessName: string;
    firstPaymentDate: Date;
    finalPaymentDate: Date | null;
    nextPaymentDate: Date;
    isActive: boolean;
}
