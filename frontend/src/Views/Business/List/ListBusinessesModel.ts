import { Vue, Component } from "vue-property-decorator";
import { IDataTableItem } from "../../../Common/DataTable/DataTableModel";
import ConfirmationDialogModel from "../../../Common/Components/ConfirmationDialog/ConfirmationDialogModel";

@Component({
    name: "ListBusinesses"
})
export default class ListBusinesses extends Vue {
    public items: IListBusinessItem[] = null!;
    public search: string = "";
    public headers: IDataTableItem[] = [
        {
            text: "",
            value: "action",
            sortable: false,
            align: "left"
        },
        {
            text: "Name",
            align: "left",
            sortable: true,
            value: "name"
        }
    ];

    public createBusiness(): void {
        this.$router.push({ name: "CreateOrEditBusiness" });
    }

    public editBusiness(item: IListBusinessItem): void {
        this.$router.push({ name: "CreateOrEditBusiness", params: { id: item.id } });
    }

    public async deleteBusiness(item: IListBusinessItem): Promise<void> {
        const confirmationDialog = this.$refs.confirmationDialog as ConfirmationDialogModel;
        const dialogResult = await confirmationDialog.open("Delete", "Are you sure?");
        if (!dialogResult) {
            return;
        }

        await this.$http.postSingleRequestAsync("business", "delete", { id: item.id });
        const index = this.items.indexOf(item);
        this.items.splice(index);
    }

    public async mounted(): Promise<void> {
        this.items = await this.prepareModelAsync();
    }

    private async prepareModelAsync(): Promise<IListBusinessItem[]> {
        const items = await this.$http.getAsync<IListBusinessItem[]>("business", "list");

        const model = items.sort((x, y) => x.name < y.name ? -1 : 1);
        return model;
    }
}

interface IListBusinessItem {
    id: string;
    name: string;
    colorKey: string;
    parentId: string;
}
