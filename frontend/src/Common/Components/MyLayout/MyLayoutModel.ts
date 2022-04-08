import Vue from "vue";
import Component from "vue-class-component";

@Component({
    name: "MyLayoutModel"
})
export default class MyLayout extends Vue {
    public drawer: boolean = true;
    public navigationItems: INavigationItem[] = [
        {
            name: "Home",
            route: "ViewHome",
            icon: "mdi-view-dashboard"
        },
        {
            name: "Money activity",
            route: "ListMoneyActivities",
            icon: "mdi-cash"
        },
        {
            name: "Categories",
            route: "ListCategories",
            icon: "mdi-package"
        },
        {
            name: "Businesses",
            route: "ListBusinesses",
            icon: "mdi-office-building"
        },
        {
            name: "Standing orders",
            route: "ListStandingOrders",
            icon: "mdi-sync"
        }
    ];

    public selectedItem: number = 0;

    public mounted(): void {
        this.drawer = true;
    }

    public navigate(name: string): void {
        if (this.$route.name === name) {
            return;
        }
        this.$router.push({ name });
    }
}

interface INavigationItem {
    name: string;
    route: string;
    icon: string;
}
