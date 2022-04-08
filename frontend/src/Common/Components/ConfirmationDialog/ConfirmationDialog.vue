<template>
    <v-dialog v-model="show" :max-width="options.width" :style="{ zIndex: options.zIndex }" @keydown.esc="cancel">
        <v-card>
            <v-toolbar dark :color="options.color" dense flat>
                <v-toolbar-title class="white--text">
                    {{ title }}
                </v-toolbar-title>
            </v-toolbar>
            <v-card-text v-show="!!message" class="pa-4">
                {{ message }}
            </v-card-text>
            <v-card-actions class="pt-0">
                <v-spacer></v-spacer>
                <v-btn color="primary darken-1" text @click.native="agree">Yes</v-btn>
                <v-btn color="grey" text @click.native="cancel">Cancel</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script lang="ts">
import ConfirmationDialog from "./ConfirmationDialogModel";
export default ConfirmationDialog;
</script>