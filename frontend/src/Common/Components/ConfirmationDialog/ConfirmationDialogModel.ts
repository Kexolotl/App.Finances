import Vue from "vue";
import Component from "vue-class-component";

@Component({
    name: "ConfirmationDialog"
})
export default class ConfirmationDialogModel extends Vue {
    public showDialog: boolean = false;

    public resolve: any = null;
    public reject: any = null;

    public message: string = "";
    public title: string = "";
    public options: IConfirmationDialogOptions = {
        color: "primary",
        width: 290
    };

    public get show(): boolean {
        return this.showDialog;
    }

    public set show(value: boolean) {
        this.showDialog = value;
        if (value === false) {
            this.cancel();
        }
    }

    public open(title: string, message: string, options?: IConfirmationDialogOptions): Promise<boolean> {
        this.showDialog = true;
        this.title = title;
        this.message = message;
        this.options = Object.assign(this.options, options);
        return new Promise((resolve, reject) => {
            this.resolve = resolve;
            this.reject = reject;
        });
    }

    public agree(): void {
        this.resolve(true);
        this.showDialog = false;
    }

    public cancel(): void {
        this.resolve(false);
        this.showDialog = false;
    }
}

export interface IConfirmationDialogOptions {
    color: string;
    width: number;
    zIndex?: number;
}
