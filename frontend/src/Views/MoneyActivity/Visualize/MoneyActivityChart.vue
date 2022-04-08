<template>
    <v-container v-if="model">
        <v-row no-gutters>
            <v-col cols="6" class="pr-2">
                <v-row no-gutters>
                    <v-col cols="12">
                        <v-menu
                            v-model="showFromDatePicker"
                            :close-on-content-click="false"
                            :nudge-right="40"
                            transition="scale-transition"
                            offset-y
                            min-width="290px">
                            <template v-slot:activator="{ on }">
                                <v-text-field
                                    v-model="fromFormatted"
                                    label="From"
                                    prepend-icon="event"
                                    readonly
                                    v-on="on"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="fromFormatted" @input="showFromDatePicker = false"></v-date-picker>
                        </v-menu>
                    </v-col>
                    <v-col cols="12">
                        <v-menu
                            v-model="showToDatePicker"
                            :close-on-content-click="false"
                            :nudge-right="40"
                            transition="scale-transition"
                            offset-y
                            min-width="290px">
                            <template v-slot:activator="{ on }">
                                <v-text-field
                                    v-model="toFormatted"
                                    label="To"
                                    prepend-icon="event"
                                    readonly
                                    v-on="on"
                                ></v-text-field>
                            </template>
                            <v-date-picker v-model="toFormatted" @input="showToDatePicker = false"></v-date-picker>
                        </v-menu>
                    </v-col>
                    <v-col cols="12">
                        <v-select
                            prepend-icon="insert_chart_outlined"
                            label="Chart type"
                            v-model="model.chartType"
                            :items="chartTypeItems"
                            item-value="value"
                            item-text="name"
                        ></v-select>
                    </v-col>
                </v-row>
            </v-col>
            <v-col cols="6" class="pl-2">
                <v-row no-gutters>
                    <v-col cols="12">
                        <v-autocomplete
                            label="Category"
                            v-model="model.categoryId"
                            :items="model.categories"
                            item-value="id"
                            item-text="name"
                            clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12">
                        <v-autocomplete
                            label="Business"
                            v-model="model.businessId"
                            :items="model.businesses"
                            item-value="id"
                            item-text="name"
                            clearable
                        ></v-autocomplete>
                    </v-col>
                    <v-col cols="12" class="text-right">
                        <v-btn @click="renderChartAsync()">Generate chart</v-btn>
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
        <!-- <highcharts :options="chartOptions"></highcharts> -->
        <div v-show="isPieChart" class="pie-chart-container" id="pieChart" ref="pieChart"></div>
        <div v-show="isXyChart" class="xy-chart-container" id="xyChart" ref="xyChart"></div>
    </v-container>
</template>
<script lang="ts">
import MoneyActivityChart from "./MoneyActivityChartModel";
export default MoneyActivityChart;
</script>
<style lang="scss" scoped>
.pie-chart-container {
    width: 100%;
    height: 65vh;
}

.xy-chart-container {
    width: 100%;
    height: 65vh;
}
</style>