import { Component } from "vue-property-decorator";
import Page from "../../../Common/Base/Page";

@Component({
    name: "CreateOrEditCategory"
})
export default class CreateOrEditCategory extends Page<ICreateOrEditCategoryResponse> {
    public back(): void {
        this.$router.push({ name: "ListCategories" });
    }

    protected async savePageAsync(): Promise<void> {
        if (this.isNew) {
            const command: ICreateCategoryCommand = {
                name: this.model.name,
                colorKey: this.model.colorKey,
                parentId: this.model.parentId != undefined ? this.model.parentId : null
            };

            await this.$http.postSingleRequestAsync("category", "create", command);
        } else {
            const command: IUpdateCategoryCommand = {
                id: this.model.id,
                name: this.model.name,
                colorKey: this.model.colorKey,
                parentId: this.model.parentId != undefined ? this.model.parentId : null
            };

            await this.$http.postSingleRequestAsync("category", "update", command);
        }
        this.back();
    }

    protected async prepareModelAsync(): Promise<ICreateOrEditCategoryResponse> {
        const id = this.$route.params.id;
        if (id == undefined) {
            this.isNew = true;
            const createItem = await this.$http.getAsync<ICreateOrEditCategoryResponse>("category", "get");
            createItem.colorKey = "#000000";
            createItem.availableCategories = createItem.availableCategories.sort((x, y) => x.name < y.name ? -1 : 1);
            return createItem;
        }

        const editItem = await this.$http.getAsync<ICreateOrEditCategoryResponse>("category", "get", { id });
        editItem.availableCategories = editItem.availableCategories.sort((x, y) => x.name < y.name ? -1 : 1);
        return editItem;
    }
}

interface ICreateOrEditCategoryResponse {
    id: string;
    name: string;
    colorKey: string;
    parentId: string;
    availableCategories: ICreateOrEditCategoryResponse[];
}

interface IUpdateCategoryCommand {
    id: string;
    name: string;
    parentId: string | null;
    colorKey: string;
}

interface ICreateCategoryCommand {
    name: string;
    parentId: string | null;
    colorKey: string;
}
