import Vue from "vue";
import HttpClient from "./Common/Http/HttpClient";
import { ChangeChecker } from "change-checker";
import { IConfirmationDialogOptions } from "./Common/Components/ConfirmationDialog/ConfirmationDialogModel";

declare module 'vue/types/vue' {
    export interface Vue {
        $http: HttpClient;
        $changeChecker: ChangeChecker;
        $showConfirmationDialogAsync: (message: string, title: string, options?: IConfirmationDialogOptions) => Promise<boolean>
    }
}
