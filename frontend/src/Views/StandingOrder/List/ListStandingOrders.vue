<template>
    <v-card v-if="items">
        <confirmation-dialog ref="confirmationDialog"></confirmation-dialog>
        <v-card-title>
            Standing orders
            <v-btn text icon color="primary" @click="createStandingOrder()">
                <v-icon>mdi-plus</v-icon>
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
                @click="editStandingOrder(item)"
            >
                mdi-pencil
            </v-icon>
            <v-icon
                small
                class="mr-2"
                @click="deleteStandingOrder(item)"
            >
                mdi-delete
            </v-icon>
        </template>
        <template v-slot:item.category="{ item }">
            <span>{{ item.category.name }}</span>
        </template>
        <template v-slot:item.business="{ item }">
            <span>{{ item.business.name }}</span>
        </template>
        <template v-slot:item.firstPaymentDate="{ item }">
            <span>{{ getFormattedDate(item.firstPaymentDate) }}</span>
        </template>
        <template v-slot:item.finalPaymentDate="{ item }">
            <span>{{ getFormattedDate(item.finalPaymentDate) }}</span>
        </template>
        <template v-slot:item.nextPaymentDate="{ item }">
            <span>{{ getFormattedDate(item.nextPaymentDate) }}</span>
        </template>
        <template v-slot:item.amount="{ item }">
            <span>{{ item.amount }}</span>
        </template>
        <template v-slot:item.isActive="{ item }">
            <v-icon v-if="item.isActive">mdi-check</v-icon>
            <v-icon v-else>mdi-close</v-icon>
        </template>
        <template v-if="items.length === 0" v-slot:no-results>
            NO RESULTS HERE!
        </template>
        </v-data-table>
    </v-card>
</template>
<script lang="ts">
import ListStandingOrders from "./ListStandingOrdersModel";
export default ListStandingOrders;
</script>