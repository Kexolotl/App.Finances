import { Component } from "vue-property-decorator";
import { ActivityType, PaymentType } from "../../../Common/Enums";
import Page from "../../..//Common/Base/Page";
import moment from "moment";
import { ValidationObserver } from "vee-validate";

@Component({
    name: "CreateOrEditMoneyActivity"
})
export default class CreateOrEditMoneyActivity extends Page<ICreateOrEditMoneyActivityResponse> {

    public get activityTypeItems(): IActivityTypeItem[] {
        return [
            { id: ActivityType.Income, name: "Income" },
            { id: ActivityType.Saving, name: "Saving" },
            { id: ActivityType.Expenditure, name: "Expenditure" }
        ];
    }

    public get paymentTypeItems(): IPaymentTypeItem[] {
        return [
            { id: PaymentType.Cash, name: "Cash" },
            { id: PaymentType.CreditCard, name: "CreditCard" },
            { id: PaymentType.GiroCard, name: "GiroCard" },
            { id: PaymentType.Paypal, name: "PayPal" }
        ];
    }

    public showCashActivityDatePicker: boolean = false;
    public showWarrantyDatePicker: boolean = false;
    public isEditAllowed: boolean = true;

    public get isExpenditure(): boolean {
        return this.model.activityType === ActivityType.Expenditure;
    }

    public get isSaving(): boolean {
        return this.model.activityType === ActivityType.Saving;
    }

    public get cashActivityDate(): string {
        return moment(this.model.cashActivityDate).format("YYYY-MM-DD");
    }

    public set cashActivityDate(value: string) {
        this.model.cashActivityDate = new Date(value);
    }

    public get warrantyPurchaseDate(): string | null {
        if (this.model.warranty?.purchaseDate == undefined) {
            return null;
        }
        return moment(this.model.warranty.purchaseDate).format("YYYY-MM-DD");
    }

    public set warrantyPurchaseDate(value: string | null) {
        this.model.warranty!.purchaseDate = value == undefined ? null : new Date(value);
    }

    public changeActivityType(): void {
        this.model.importantForTax = null;
        this.model.warranty = null;
        if (this.model.activityType === ActivityType.Expenditure) {
            this.model.importantForTax = false;
            this.model.warranty = {
                lengthInMonth: null!,
                purchaseDate: null
            };
        }
    }

    public back(): void {
        this.$router.push({ name: "ListMoneyActivities" });
    }

    protected async savePageAsync(): Promise<void> {
        const observer = this.$refs.defaultObserver as InstanceType<typeof ValidationObserver>;
        const isValid = await observer.validate();
        if (!isValid) {
            return;
        }

        if (this.isNew) {
            switch (this.model.activityType) {
                case ActivityType.Expenditure:
                    const warranty: ICreateWarranty | null = this.model.warranty != undefined &&
                        this.model.warranty.lengthInMonth != undefined &&
                        this.model.warranty.purchaseDate != undefined ?
                        { lengthInMonth: this.model.warranty.lengthInMonth, purchaseDate: this.model.warranty.purchaseDate } :
                        null;

                    const expenditureCommand: ICreateExpenditureMoneyActivityCommand = {
                        amount: this.model.amount,
                        description: this.model.description != undefined && this.model.description.trim() !== "" ? this.model.description : null,
                        businessId: this.model.businessId != undefined ? this.model.businessId : null,
                        categoryId: this.model.categoryId!,
                        cashActivityDate: this.model.cashActivityDate,
                        importantForTax: this.model.importantForTax ?? null,
                        paymentType: this.model.paymentType,
                        warranty
                    };

                    await this.$http.postSingleRequestAsync("MoneyActivity", "CreateExpenditure", expenditureCommand);
                    break;
                case ActivityType.Income:
                    const incomeCommand: ICreateIncomeMoneyActivityCommand = {
                        amount: this.model.amount,
                        description: this.model.description != undefined && this.model.description.trim() !== "" ? this.model.description : null,
                        businessId: this.model.businessId != undefined ? this.model.businessId : null,
                        categoryId: this.model.categoryId!,
                        cashActivityDate: this.model.cashActivityDate,
                        paymentType: this.model.paymentType
                    };

                    await this.$http.postSingleRequestAsync("MoneyActivity", "CreateIncome", incomeCommand);
                    break;
                case ActivityType.Saving:
                    const savingCommand: ICreateSavingMoneyActivityCommand = {
                        amount: this.model.amount,
                        description: this.model.description != undefined && this.model.description.trim() !== "" ? this.model.description : null,
                        businessId: this.model.businessId != undefined ? this.model.businessId : null,
                        categoryId: this.model.categoryId!,
                        cashActivityDate: this.model.cashActivityDate,
                        paymentType: this.model.paymentType
                    };

                    await this.$http.postSingleRequestAsync("MoneyActivity", "CreateSaving", savingCommand);
                    break;
            }
        } else {
            let warranty: IUpdateWarranty | null = null;
            if (this.model.activityType === ActivityType.Expenditure) {
                warranty = this.model.warranty != undefined && this.model.warranty.purchaseDate != undefined ?
                    { lengthInMonth: this.model.warranty.lengthInMonth == undefined || this.model.warranty.lengthInMonth.toString().trim() === "" ? null : Number(this.model.warranty.lengthInMonth), purchaseDate: this.model.warranty.purchaseDate } :
                    null;
            }
            const updateCommand: IUpdateMoneyActivityCommand = {
                id: this.model.id,
                amount: this.model.amount,
                description: this.model.description != undefined && this.model.description.trim() !== "" ? this.model.description : null,
                businessId: this.model.businessId != undefined ? this.model.businessId : null,
                categoryId: this.model.categoryId!,
                cashActivityDate: this.model.cashActivityDate,
                activityType: this.model.activityType,
                importantForTax: this.model.importantForTax == undefined ? null : this.model.importantForTax,
                warranty
            };

            await this.$http.postSingleRequestAsync("MoneyActivity", "UpdateMoneyActivity", updateCommand);
        }

        this.back();
    }

    protected async prepareModelAsync(): Promise<ICreateOrEditMoneyActivityResponse> {
        const id = this.$route.params.id;
        if (id == undefined) {
            this.isNew = true;
            const createItem = await this.$http.getAsync<ICreateOrEditMoneyActivityResponse>("MoneyActivity", "get");
            createItem.cashActivityDate = new Date();
            createItem.paymentType = PaymentType.GiroCard;
            createItem.activityType = ActivityType.Expenditure;
            createItem.warranty = { lengthInMonth: null, purchaseDate: null };
            createItem.importantForTax = false;

            createItem.availableBusinesses = createItem.availableBusinesses.sort((x, y) => x.name < y.name ? -1 : 1);
            createItem.availableCategories = createItem.availableCategories.sort((x, y) => x.name < y.name ? -1 : 1);
            return createItem;
        }
        this.isEditAllowed = false;

        const editItem = await this.$http.getAsync<ICreateOrEditMoneyActivityResponse>("MoneyActivity", "get", { id });
        editItem.availableBusinesses = editItem.availableBusinesses.sort((x, y) => x.name < y.name ? -1 : 1);
        editItem.availableCategories = editItem.availableCategories.sort((x, y) => x.name < y.name ? -1 : 1);
        if (editItem.warranty == undefined) {
            editItem.warranty = {
                lengthInMonth: null!,
                purchaseDate: null!
            };
        }
        return editItem;
    }
}

interface ICreateOrEditMoneyActivityResponse {
    id: string;
    amount: string;
    description: string | null;
    businessId: string | null;
    categoryId: string | null;
    cashActivityDate: Date;
    importantForTax: boolean | null;
    warranty: IWarrantyResponse | null;
    paymentType: PaymentType;
    activityType: ActivityType;
    availableBusinesses: IBusinessResponse[];
    availableCategories: ICategoryResponse[];
}

interface IWarrantyResponse {
    lengthInMonth: number | null;
    purchaseDate: Date | null;
}

interface IPaymentTypeItem {
    id: number;
    name: string;
}

interface IActivityTypeItem {
    id: number;
    name: string;
}

interface IBusinessResponse {
    id: string;
    name: string;
}

interface ICategoryResponse {
    id: string;
    name: string;
}

interface IUpdateMoneyActivityCommand {
    id: string;
    amount: string;
    description: string | null;
    businessId: string | null;
    categoryId: string;
    cashActivityDate: Date;
    activityType: ActivityType;
    warranty: IUpdateWarranty | null;
    importantForTax: boolean | null;
}

interface IUpdateWarranty {
    lengthInMonth: number | null;
    purchaseDate: Date;
}

interface ICreateExpenditureMoneyActivityCommand {
    amount: string;
    description: string | null;
    businessId: string | null;
    categoryId: string;
    cashActivityDate: Date;
    paymentType: PaymentType;
    importantForTax: boolean | null;
    warranty: ICreateWarranty | null;
}

interface ICreateSavingMoneyActivityCommand {
    amount: string;
    description: string | null;
    businessId: string | null;
    categoryId: string;
    cashActivityDate: Date;
    paymentType: PaymentType;
}

interface ICreateIncomeMoneyActivityCommand {
    amount: string;
    description: string | null;
    businessId: string | null;
    categoryId: string;
    cashActivityDate: Date;
    paymentType: PaymentType;
}

interface ICreateWarranty {
    lengthInMonth: number | null;
    purchaseDate: Date;
}
