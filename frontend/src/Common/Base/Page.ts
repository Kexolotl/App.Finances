import Vue from "vue";
import { Watch, Component } from "vue-property-decorator";
import { ObjectDiff } from "change-checker";

@Component({
    name: "PageBase"
})
export default class Page<TModel extends object> extends Vue {
    public model: TModel = null!;
    public isDirty: boolean = false;
    public isNew: boolean = false;

    private snapshot: TModel = null!;

    @Watch("model", { deep: true })
    public onModelChanged(_: TModel, oldValue: TModel): void {
        if (oldValue == undefined || this.model == undefined || this.snapshot == undefined) {
            this.isDirty = false;
            return;
        }

        const diff = this.$changeChecker.createDiff(this.snapshot, this.model);
        this.isDirty = diff.$isDirty();
    }

    public async mounted(): Promise<void> {
        try {
            this.model = await this.prepareModelAsync();
            if (this.model == undefined) {
                return;
            }
            this.snapshot = this.$changeChecker.takeSnapshot(this.model);
        } catch (error) {
            alert(error);
        }
        await this.onPageMounted();
    }

    public async saveAsync(): Promise<boolean> {
        if (this.isDirty) {
            const diff = this.$changeChecker.createDiff(this.snapshot, this.model);
            await this.savePageAsync(this.model, diff);
            this.isDirty = false;
            this.isNew = false;
            return true;
        }
        return false;
    }

    protected async prepareModelAsync(): Promise<TModel> {
        throw new Error("Function must be overridden");
    }

    /**
     * Required
     * @param model Your model
     * @param diff Your diff of your model
     */
    protected async savePageAsync(model: TModel, diff: ObjectDiff<TModel>): Promise<void> {
        throw new Error("Function must be overridden with " + model + " and " + diff);
    }

    protected onPageMounted(): Promise<void> | void {
        return;
    }

    protected onBeforePageDestroyed(): Promise<void> | void {
        return;
    }
}
