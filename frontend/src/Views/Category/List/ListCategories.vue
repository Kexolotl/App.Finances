<template>
    <v-card v-if="items">
        <confirmation-dialog ref="confirmationDialog"></confirmation-dialog>
        <v-card-title>
            Categories
            <v-btn text icon color="primary" @click="createCategory()">
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
                @click="editCategory(item)"
            >
                mdi-pencil
            </v-icon>
            <v-icon
                small
                class="mr-2"
                @click="deleteCategory(item)"
            >
                mdi-delete
            </v-icon>
        </template>
        <!--
            TODO for future work
            <template v-slot:item="row">
                <template v-for="(item, rowIndex) of items">
                    <td v-for="(rowAction, actionIndex) of rowActions" v-bind:key="'action'+rowIndex+actionIndex">
                        <v-icon @click="rowAction.action(item)">{{rowAction.icon}}</v-icon>
                    </td>
                    
                    <template v-for="(header, collIndex) of headers">
                        <td v-if="header.value != undefined" v-bind:key="'cell'+rowIndex + collIndex">
                            {{ item[header.value] }}
                        </td>
                    </template>
                </template>
                
            </template>
        -->
        <template v-if="items.length === 0" v-slot:no-results>
            NO RESULTS HERE!
        </template>
        </v-data-table>
    </v-card>
</template>
<script lang="ts">
import ListCategories from "./ListCategoriesModel";
export default ListCategories;
</script>