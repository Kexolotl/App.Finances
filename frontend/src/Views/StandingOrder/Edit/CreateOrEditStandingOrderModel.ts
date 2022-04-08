import { Component } from "vue-property-decorator";
import { ActivityType, FrequencyType, PaymentType } from "../../../Common/Enums";
import Page from "../../..//Common/Base/Page";
import moment from "moment";
import { ValidationObserver } from "vee-validate";

@Component({
    name: "CreateOrEditMoneyActivity"
})
export default class CreateOrEditMoneyActivity extends Page<ICreateOrEditStandingOrderResponse> {

    public get activityTypeItems(): Array<{ id: ActivityType, name: string }> {
        return [
            { id: ActivityType.Income, name: "Income" },
            { id: ActivityType.Saving, name: "Saving" },
            { id: ActivityType.Expenditure, name: "Expenditure" }
        ];
    }

    public get frequencyTypeItems(): Array<{ id: FrequencyType, name: string }> {
        return [
            { id: FrequencyType.Daily, name: "Daily" },
            { id: FrequencyType.Monthly, name: "Monthly" },
            { id: FrequencyType.Yearly, name: "Yearly" }
        ];
    }

    public get paymentTypeItems(): Array<{ id: PaymentType, name: string }> {
        return [
            { id: PaymentType.Cash, name: "Cash" },
            { id: PaymentType.CreditCard, name: "CreditCard" },
            { id: PaymentType.GiroCard, name: "GiroCard" },
            { id: PaymentType.Paypal, name: "PayPal" }
        ];
    }

    public showFirstPaymentDatePicker: boolean = false;
    public showFinalPaymentDatePicker: boolean = false;
    public isEditAllowed: boolean = true;

    public get isExpenditure(): boolean {
        return this.model.activityType === ActivityType.Expenditure;
    }

    public get firstPaymentDate(): string {
        return moment(this.model.firstPaymentDate).format("YYYY-MM-DD");
    }

    public set firstPaymentDate(value: string) {
        this.model.firstPaymentDate = new Date(value);
    }

    public get finalPaymentDate(): string | null {
        if (this.model.finalPaymentDate == undefined) {
            return null;
        }
        return moment(this.model.finalPaymentDate).format("YYYY-MM-DD");
    }

    public set finalPaymentDate(value: string | null) {
        if (value == undefined) {
            this.model.finalPaymentDate = null;
            return;
        }

        this.model.finalPaymentDate = new Date(value);
    }

    public changeActivityType(): void {
        this.model.importantForTax = null;
        if (this.model.activityType === ActivityType.Expenditure) {
            this.model.importantForTax = false;
        }
    }

    public back(): void {
        this.$router.push({ name: "ListStandingOrders" });
    }

    protected async savePageAsync(): Promise<void> {
        const observer = this.$refs.defaultObserver as InstanceType<typeof ValidationObserver>;
        const isValid = await observer.validate();
        if (!isValid) {
            return;
        }

        if (this.isNew) {
            const createStandingOrderCommand: ICreateStandingOrderCommand = {
                amount: this.model.amount,
                businessId: this.model.business != undefined ? this.model.business.id : null,
                categoryId: this.model.category!.id,
                firstPaymentDate: this.model.firstPaymentDate,
                finalPaymentDate: this.model.finalPaymentDate,
                importantForTax: this.model.importantForTax ?? null,
                paymentType: this.model.paymentType,
                frequency: this.model.frequency
            };

            await this.$http.postSingleRequestAsync("StandingOrder", "Create", createStandingOrderCommand);
        } else {
            const updateCommand: IUpdateStandingOrderCommand = {
                id: this.model.id,
                isActive: this.model.isActive,
                amount: this.model.amount,
                finalPaymentDate: this.model.finalPaymentDate
            };

            await this.$http.postSingleRequestAsync("StandingOrder", "UpdateStandingOrder", updateCommand);
        }

        this.back();
    }

    protected async prepareModelAsync(): Promise<ICreateOrEditStandingOrderResponse> {
        const id = this.$route.params.id;
        if (id == undefined) {
            this.isNew = true;
            const createItem = await this.$http.getAsync<ICreateOrEditStandingOrderResponse>("StandingOrder", "get");
            createItem.firstPaymentDate = new Date();
            createItem.paymentType = PaymentType.GiroCard;
            createItem.activityType = ActivityType.Expenditure;
            createItem.frequency = FrequencyType.Daily;
            createItem.importantForTax = false;

            createItem.businesses = createItem.businesses.sort((x, y) => x.name < y.name ? -1 : 1);
            createItem.categories = createItem.categories.sort((x, y) => x.name < y.name ? -1 : 1);
            return createItem;
        }
        this.isEditAllowed = false;

        const editItem = await this.$http.getAsync<ICreateOrEditStandingOrderResponse>("StandingOrder", "get", { id });
        editItem.businesses = editItem.businesses.sort((x, y) => x.name < y.name ? -1 : 1);
        editItem.categories = editItem.categories.sort((x, y) => x.name < y.name ? -1 : 1);
        return editItem;
    }
}

interface ICreateOrEditStandingOrderResponse {
    id: string;
    amount: string;
    business: IBusinessResponse | null;
    category: ICategoryResponse | null;
    firstPaymentDate: Date;
    finalPaymentDate: Date | null;
    importantForTax: boolean | null;
    isActive: boolean;
    paymentType: PaymentType;
    activityType: ActivityType;
    frequency: FrequencyType;
    businesses: IBusinessResponse[];
    categories: ICategoryResponse[];
}

interface IBusinessResponse {
    id: string;
    name: string;
}

interface ICategoryResponse {
    id: string;
    name: string;
}

interface IUpdateStandingOrderCommand {
    id: string;
    isActive: boolean;
    amount: string;
    finalPaymentDate: Date | null;
}

interface ICreateStandingOrderCommand {
    amount: string;
    businessId: string | null;
    categoryId: string;
    firstPaymentDate: Date;
    finalPaymentDate: Date | null;
    paymentType: PaymentType;
    importantForTax: boolean | null;
    frequency: FrequencyType;
}
