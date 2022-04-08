<template>
    <v-card v-if="items">
        <confirmation-dialog ref="confirmationDialog"></confirmation-dialog>
        <v-card-title>
            Businesses
            <v-btn text icon color="primary" @click="createBusiness()">
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
            :footer-props="{
                'items-per-page-options': [10, 20, 30, 40, 50, -1]
            }"
            :items-per-page="20"
            dense
            :headers="headers"
            :items="items"
            :search="search"
        >
        <template v-slot:item.action="{ item }">
            <v-icon
                small
                class="mr-2"
                @click="editBusiness(item)"
            >
                mdi-pencil
            </v-icon>
            <v-icon
                small
                class="mr-2"
                @click="deleteBusiness(item)"
            >
                mdi-delete
            </v-icon>
        </template>
        <template v-if="items.length === 0" v-slot:no-results>
            NO RESULTS HERE!
        </template>
        </v-data-table>
    </v-card>
</template>
<script lang="ts">
import ListBusinesses from "./ListBusinessesModel";
export default ListBusinesses;
</script>