import { Component } from "vue-property-decorator";
import Page from "@/Common/Base/Page";
import moment from "moment";

import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_dark from "@amcharts/amcharts4/themes/dark";
import Decimal from "decimal.js";

@Component({
    name: "MoneyActivityChart"
})
export default class MoneyActivityChart extends Page<IMoneyActivityChartOverviewResposne> {
    public selectedCategoryId: string = null!;
    public selectedBusinessId: string | null = null;

    public showFromDatePicker: boolean = false;
    public showToDatePicker: boolean = false;

    public chartRegistration: Record<string, any> = {};

    public get isPieChart(): boolean {
        return this.model.chartType === ChartTypes.PieChart;
    }

    public get isXyChart(): boolean {
        return this.model.chartType === ChartTypes.XyChart;
    }

    public get toFormatted(): string {
        return moment(this.model.to).format("YYYY-MM-DD");
    }

    public set toFormatted(value: string) {
        this.model.to = new Date(value);
    }

    public get fromFormatted(): string {
        return moment(this.model.from).format("YYYY-MM-DD");
    }

    public set fromFormatted(value: string) {
        this.model.from = new Date(value);
    }

    public get chartTypeItems(): Array<{ value: ChartTypes, name: string }> {
        return [
            {
                name: "Pie chart",
                value: ChartTypes.PieChart
            },
            {
                name: "XY chart",
                value: ChartTypes.XyChart
            }
        ];
    }

    public async renderChartAsync(): Promise<void> {
        switch (this.model.chartType) {
            case ChartTypes.PieChart:

                // Check for any instance
                this.maybeDisposeChart("pieChart");

                try {
                    // TODO: Define Color in chartData -> https://www.amcharts.com/docs/v4/chart-types/pie-chart/

                    const params: Record<string, string> = {
                        from: this.fromFormatted,
                        to: this.toFormatted
                    };

                    if (this.model.businessId != undefined) {
                        params.businessId = this.model.businessId;
                    }

                    if (this.model.categoryId != undefined) {
                        params.categoryId = this.model.categoryId;
                    }

                    // Create chart
                    const chartData = await this.$http.getAsync<IMoneyActivityPieChartDataResposne>(
                        "MoneyActivityChart",
                        "GetMoneyActivityPieChartData",
                        params
                    );

                    if (chartData.moneyActivities.length === 0) {
                        return;
                    }

                    am4core.useTheme(am4themes_dark);

                    const pieChartDiv = this.$refs.pieChart as HTMLElement;
                    const pieChart = am4core.create(pieChartDiv, am4charts.PieChart);
                    pieChart.innerRadius = am4core.percent(10);
                    pieChart.legend = new am4charts.Legend();

                    this.chartRegistration.pieChart = pieChart;

                    pieChart.data = chartData.moneyActivities;

                    // Add and configure Series
                    const pieSeries = pieChart.series.push(new am4charts.PieSeries());
                    pieSeries.dataFields.value = "amount";
                    pieSeries.dataFields.category = "category";
                    pieSeries.slices.template.stroke = am4core.color("#6e6f70");
                    pieSeries.slices.template.strokeWidth = 2;
                    pieSeries.slices.template.strokeOpacity = 0.5;
                    pieSeries.labels.template.disabled = true;
                    pieSeries.ticks.template.disabled = true;
                } catch (error) {
                    throw new Error(error as any);
                }
                break;
            case ChartTypes.XyChart:

                // Check for any instance
                this.maybeDisposeChart("xyChart");

                try {
                    // TODO: Define Color in chartData -> https://www.amcharts.com/docs/v4/chart-types/pie-chart/
                    const params: Record<string, string> = {
                        from: this.fromFormatted,
                        to: this.toFormatted
                    };

                    if (this.model.businessId != undefined) {
                        params.businessId = this.model.businessId;
                    }

                    if (this.model.categoryId != undefined) {
                        params.categoryId = this.model.categoryId;
                    }

                    // Create chart
                    const chartData = await this.$http.getAsync<IMoneyActivityXyChartDataResposne>(
                        "MoneyActivityChart",
                        "GetMoneyActivityXyChartData",
                        params
                    );

                    if (chartData.moneyActivities.length === 0) {
                        return;
                    }

                    chartData.moneyActivities = chartData.moneyActivities.sort((a, b) => new Decimal(a.amount).lessThan(new Decimal(b.amount)) ? -1 : 1);

                    am4core.useTheme(am4themes_dark);

                    const xyChartDiv = this.$refs.xyChart as HTMLElement;
                    const xyChart = am4core.create(xyChartDiv, am4charts.XYChart);
                    this.chartRegistration.xyChart = xyChart;
                    xyChart.marginRight = 400;

                    xyChart.data = chartData.moneyActivities;

                    // Create category axes
                    const categoryAxis = xyChart.yAxes.push(new am4charts.CategoryAxis());
                    categoryAxis.dataFields.category = "category";
                    categoryAxis.renderer.grid.template.location = 0;
                    categoryAxis.renderer.minGridDistance = 10;

                    // create value axis
                    const valueAxis = xyChart.xAxes.push(new am4charts.ValueAxis());
                    valueAxis.title.text = "Amount";

                    const series = xyChart.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = "amount";
                    series.dataFields.categoryY = "category";
                    series.tooltipText = "{category}: [bold]{valueX} €[/]";
                    series.stacked = true;

                    xyChart.cursor = new am4charts.XYCursor();
                } catch (error) {
                    throw new Error(error as any);
                }
                break;
        }
    }

    protected async prepareModelAsync(): Promise<IMoneyActivityChartOverviewResposne> {
        const model = await this.$http.getAsync<IMoneyActivityChartOverviewResposne>("MoneyActivityChart", "GetMoneyActivityChartOverview");

        const from = new Date();
        from.setMonth(from.getMonth() - 1);
        model.to = new Date();
        model.from = from;
        model.chartType = ChartTypes.PieChart;
        model.businessId = null;
        model.businesses = model.businesses.sort((x, y) => x.name < y.name ? -1 : 1);
        model.categories = model.categories.sort((x, y) => x.name < y.name ? -1 : 1);

        return model;
    }

    private maybeDisposeChart(chartdiv: string): void {
        if (this.chartRegistration[chartdiv] == undefined) {
            return;
        }
        this.chartRegistration[chartdiv].dispose();
        this.chartRegistration[chartdiv] = null;
    }
}

interface IMoneyActivityChartOverviewResposne {
    categories: ICategoryResponse[];
    businesses: IBusinessResponse[];
    categoryId: string | null;
    businessId: string | null;
    chartType: ChartTypes;
    from: Date;
    to: Date;
}

interface IMoneyActivityPieChartDataResposne {
    moneyActivities: IMoneyActivityResponse[];
}

interface IMoneyActivityXyChartDataResposne {
    moneyActivities: IMoneyActivityResponse[];
}

interface IMoneyActivityResponse {
    amount: string;
    category: string;
}

interface IBusinessResponse {
    id: string;
    name: string;
}

interface ICategoryResponse {
    id: string;
    name: string;
}

enum ChartTypes {
    PieChart,
    XyChart
}
