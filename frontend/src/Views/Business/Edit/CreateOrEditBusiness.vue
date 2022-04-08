<template>
    <div v-if="model">
        <v-card v-if="model">
            <v-toolbar dark color="primary">
            <v-btn icon dark @click="back()">
                <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
            <v-toolbar-title>Business</v-toolbar-title>
            <v-spacer></v-spacer>
                <v-btn icon dark @click="saveAsync()">
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
            </v-row>
            </v-card-text>
        </v-card>
    </div>
</template>
<script lang="ts">
import CreateOrEditBusiness from "./CreateOrEditBusinessModel";
export default CreateOrEditBusiness;
</script>