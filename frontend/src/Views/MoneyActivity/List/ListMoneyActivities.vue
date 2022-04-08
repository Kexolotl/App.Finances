<template>
    <v-card v-if="items">
        <confirmation-dialog ref="confirmationDialog"></confirmation-dialog>
        <v-card-title>
            Money activities
            <v-btn text icon color="primary" @click="createMoneyActivity()">
                <v-icon>mdi-plus</v-icon>
            </v-btn>
            <v-btn text icon color="primary" @click="() => showStatistic()">
                <v-icon>mdi-chart-bar</v-icon>
            </v-btn>
        <v-spacer></v-spacer>
        <v-text-field
            v-model="search"
            append-icon="search"
            label="Search"
            single-line
            hide-details
        ></v-text-field>
        </v-card-title>
        <v-data-table
            dense
            :footer-props="{
                'items-per-page-options': [10, 20, 30, 40, 50, -1]
            }"
            :items-per-page="20"
            :headers="headers"
            :items="items"
            :search="search"
        >
        <template v-slot:item.action="{ item }">
            <v-icon
                small
                class="mr-2"
                @click="editMoneyActivity(item)"
            >
                mdi-pencil
            </v-icon>
            <v-icon
                small
                class="mr-2"
                @click="deleteMoneyActivity(item)"
            >
                mdi-delete
            </v-icon>
        </template>
        <template v-slot:item.paymentType="{ item }">
            {{ getPaymentTypeAsString(item.paymentType) }}
        </template>
        <template v-slot:item.cashActivityDate="{ item }">
            {{ item.cashActivityDate | formatDate }}
        </template>
        <template v-slot:item.activityType="{ item }">
            {{ getActivityTypeAsString(item.activityType) }}
        </template>
        <template v-slot:item.amount="{ item }">
            <span v-if="isIncomeActivityType(item.activityType)" class="success--text">{{ item.amount }}</span>
            <span v-if="isSavingActivityType(item.activityType)" class="warning--text">{{ item.amount }}</span>
            <span v-if="isExpenditureActivityType(item.activityType)" class="error--text">{{ item.amount }}</span>
        </template>
        <template v-if="items.length === 0" v-slot:no-results>
            NO RESULTS HERE!
        </template>
        </v-data-table>
    </v-card>
</template>
<script lang="ts">
import ListMoneyActivities from "./ListMoneyActivitiesModel";
export default ListMoneyActivities;
</script>