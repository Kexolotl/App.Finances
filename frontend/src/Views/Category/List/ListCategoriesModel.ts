import { Vue, Component } from "vue-property-decorator";
import { IDataTableItem } from "../../../Common/DataTable/DataTableModel";
import ConfirmationDialogModel from "../../../Common/Components/ConfirmationDialog/ConfirmationDialogModel";

@Component({
    name: "ListCategories"
})
export default class ListCategories extends Vue {
    public items: IListCategoryItem[] = null!;
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
        },
        {
            text: "Color key",
            align: "left",
            sortable: false,
            value: "colorKey"
        }
    ];

    public createCategory(): void {
        this.$router.push({ name: "CreateOrEditCategory" });
    }

    public editCategory(item: IListCategoryItem): void {
        this.$router.push({ name: "CreateOrEditCategory", params: { id: item.id } });
    }

    public async deleteCategory(item: IListCategoryItem): Promise<void> {
        const confirmationDialog = this.$refs.confirmationDialog as ConfirmationDialogModel;
        const dialogResult = await confirmationDialog.open("Delete", "Are you sure?");
        if (!dialogResult) {
            return;
        }

        await this.$http.postSingleRequestAsync("category", "delete", { id: item.id });
        const index = this.items.indexOf(item);
        this.items.splice(index, 1);
    }

    public async mounted(): Promise<void> {
        this.items = await this.prepareModelAsync();
    }

    private async prepareModelAsync(): Promise<IListCategoryItem[]> {
        const items = await this.$http.getAsync<IListCategoryItem[]>("category", "list");
        const model = items.sort((x, y) => x.name < y.name ? -1 : 1);
        return model;
    }
}

interface IListCategoryItem {
    id: string;
    name: string;
    colorKey: string;
    parentId: string;
}
