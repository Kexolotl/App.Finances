import { Component } from "vue-property-decorator";
import Page from "../../../Common/Base/Page";

@Component({
    name: "CreateOrEditBusiness"
})
export default class CreateOrEditBusiness extends Page<ICreateOrEditBusinessResponse> {

    public back(): void {
        this.$router.push({ name: "ListBusinesses" });
    }

    protected async savePageAsync(): Promise<void> {
        if (this.isNew) {
            const command: ICreateBusinessCommand = {
                name: this.model.name
            };

            await this.$http.postSingleRequestAsync("business", "create", command);
        } else {
            const command: IUpdateBusinessCommand = {
                id: this.model.id,
                name: this.model.name
            };

            await this.$http.postSingleRequestAsync("business", "update", command);
        }
        this.back();
    }

    protected async prepareModelAsync(): Promise<ICreateOrEditBusinessResponse> {
        const id = this.$route.params.id;
        if (id == undefined) {
            this.isNew = true;
            return {
                id: "",
                name: ""
            };
        }

        const editItem = await this.$http.getAsync<ICreateOrEditBusinessResponse>("business", "get", { id });
        return editItem;
    }
}

interface ICreateOrEditBusinessResponse {
    id: string;
    name: string;
}

interface IUpdateBusinessCommand {
    id: string;
    name: string;
}

interface ICreateBusinessCommand {
    name: string;
}
