<template>
    <div v-if="model">
        <v-card v-if="model">
            <v-toolbar dark color="primary">
            <v-btn icon dark @click="back()">
                <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
            <v-toolbar-title>Category</v-toolbar-title>
            <v-spacer></v-spacer>
                <v-btn icon dark @click="saveAsync()" :disabled="!isDirty">
                    <v-icon>mdi-content-save</v-icon>
                </v-btn>
            </v-toolbar>
            <v-card-text>
            <v-row cols="6">
            <v-col>
                <v-text-field
                    label="Name"
                    single-line
                    v-model="model.name"
                ></v-text-field>
            </v-col>
            <v-col>
                <v-text-field
                    label="Color key"
                    single-line
                    v-model="model.colorKey"
                ></v-text-field>
            </v-col>
            </v-row>
            <v-row cols="6">
            <v-col>
                <v-autocomplete
                    label="Parent"
                    single-line
                    v-model="model.parentId"
                    :items="model.availableCategories"
                    item-value="id"
                    item-text="name"
                    clearable
                ></v-autocomplete>
            </v-col>
            </v-row>
            </v-card-text>
        </v-card>
    </div>
</template>
<script lang="ts">
import CreateOrEditCategory from "./CreateOrEditCategoryModel";
export default CreateOrEditCategory;
</script>