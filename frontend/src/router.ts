import Vue from "vue";
import Router, { RouteConfig } from "vue-router";

export default class MyRouter {

	public static myRoutes: IRoutes;

	public static initialize(): Router {
		Vue.use(Router);
		const router = new Router();

		this.myRoutes = {
			viewHome: {
				path: "/",
				name: "ViewHome",
				component: () => import(/* webpackChunkName: "home" */ "./Views/Home.vue")
			},
			listCategories: {
				path: "/categories",
				name: "ListCategories",
				component: () => import(/* webpackChunkName: "categories" */ "./Views/Category/List/ListCategories.vue")
			},
			createOrEditCategory: {
				path: "/category/createoredit/:id?",
				name: "CreateOrEditCategory",
				component: () => import(/* webpackChunkName: "categories" */ "./Views/Category/Edit/CreateOrEditCategory.vue")
			},
			listBusinesses: {
				path: "/businesses",
				name: "ListBusinesses",
				component: () => import(/* webpackChunkName: "businesses" */ "./Views/Business/List/ListBusinesses.vue")
			},
			createOrEditBusiness: {
				path: "/business/createoredit/:id?",
				name: "CreateOrEditBusiness",
				component: () => import(/* webpackChunkName: "businesses" */ "./Views/Business/Edit/CreateOrEditBusiness.vue")
			},
			listMoneyActivities: {
				path: "/moneyactivities",
				name: "ListMoneyActivities",
				component: () => import(/* webpackChunkName: "moneyactivity" */ "./Views/MoneyActivity/List/ListMoneyActivities.vue")
			},
			createOrEditMoneyActivity: {
				path: "/moneyactivity/createoredit/:id?",
				name: "CreateOrEditMoneyActivity",
				component: () => import(/* webpackChunkName: "moneyactivity" */ "./Views/MoneyActivity/Edit/CreateOrEditMoneyActivity.vue")
			},
			visualizeMoneyActivity: {
				path: "/moneyactivity/visualize/",
				name: "VisualizeMoneyActivity",
				component: () => import(/* webpackChunkName: "moneyactivity" */ "./Views/MoneyActivity/Visualize/MoneyActivityChart.vue")
			},
			createOrEditStandingOrder: {
				path: "/standingorder/createoredit/:id?",
				name: "CreateOrEditStandingOrder",
				component: () => import(/* webpackChunkName: "standingorder" */ "./Views/StandingOrder/Edit/CreateOrEditStandingOrder.vue")
			},
			listStandingOrders: {
				path: "/standingorders",
				name: "ListStandingOrders",
				component: () => import(/* webpackChunkName: "standingorder" */ "./Views/StandingOrder/List/ListStandingOrders.vue")
			}
		};

		const routes: RouteConfig[] = Object.values(this.myRoutes);
		router.addRoutes(routes);
		return router;
	}

}

export interface IRoutes {
	viewHome: RouteConfig;

	listCategories: RouteConfig;
	createOrEditCategory: RouteConfig;

	listBusinesses: RouteConfig;
	createOrEditBusiness: RouteConfig;

	listMoneyActivities: RouteConfig;
	createOrEditMoneyActivity: RouteConfig;
	visualizeMoneyActivity: RouteConfig;

	listStandingOrders: RouteConfig;
	createOrEditStandingOrder: RouteConfig;
}
