<template>
	<div v-if="model">
		<validation-observer ref="defaultObserver">
		<v-card v-if="model">
			<v-toolbar dark color="primary">
			<v-btn icon dark @click="back()">
				<v-icon>mdi-arrow-left</v-icon>
			</v-btn>
			<v-toolbar-title>Money activity</v-toolbar-title>
			<v-spacer></v-spacer>
				<v-btn icon dark @click="saveAsync()">
					<v-icon>mdi-content-save</v-icon>
				</v-btn>
			</v-toolbar>
			<v-card-text>
			<v-row cols="6">
			<v-col>
				<v-text-field
					label="Amount"
					v-model="model.amount"
				></v-text-field>
			</v-col>
			<v-col>
				<v-menu
					v-model="showCashActivityDatePicker"
					:close-on-content-click="false"
					:nudge-right="40"
					transition="scale-transition"
					offset-y
					min-width="290px">
					<template v-slot:activator="{ on }">
					<v-text-field
						v-model="cashActivityDate"
						label="Cash activity date"
						prepend-icon="event"
						readonly
						v-on="on"
					></v-text-field>
					</template>
					<v-date-picker v-model="cashActivityDate" @input="showCashActivityDatePicker = false"></v-date-picker>
				</v-menu>
			</v-col>
			</v-row>

			<v-row cols="6">
			<v-col>
				<validation-provider rules="required" name="Category" v-slot="{ errors }">
					<v-autocomplete
						:error-messages="errors[0]"
						label="Category"
						v-model="model.categoryId"
						:items="model.availableCategories"
						item-value="id"
						item-text="name"
						clearable
					></v-autocomplete>
				</validation-provider>
			</v-col>
			<v-col>
				<v-autocomplete
					label="Business"
					v-model="model.businessId"
					:items="model.availableBusinesses"
					item-value="id"
					item-text="name"
					clearable
				></v-autocomplete>
			</v-col>
			</v-row>

			<v-row cols="6">
			<v-col>
				<v-text-field
					label="Description"
					v-model="model.description"
				></v-text-field>
			</v-col>
			<v-col>
				<v-checkbox v-model="model.importantForTax" :disabled="isSaving" label="Important for tax"></v-checkbox>
			</v-col>
			</v-row>

			<v-row cols="6">
				<v-card-title>Types</v-card-title>
			</v-row>
			<v-row cols="6">
			<v-col>
				<v-select
					label="Payment type"
					v-model="model.paymentType"
					:items="paymentTypeItems"
					item-value="id"
					item-text="name"
					:disabled="!isEditAllowed"
				></v-select>
			</v-col>
			<v-col>
				<v-select
					label="Activity type"
					v-model="model.activityType"
					:items="activityTypeItems"
					item-value="id"
					item-text="name"
					@change="changeActivityType()"
					:disabled="!isEditAllowed"
				></v-select>
			</v-col>
			</v-row>

			<v-row cols="6" v-if="isExpenditure">
				<v-card-title>Warranty</v-card-title>
			</v-row>
			<v-row cols="6" v-if="isExpenditure">
			<v-col>
				<v-text-field
					label="Warranty in month"
					v-model="model.warranty.lengthInMonth"
				></v-text-field>
			</v-col>
			<v-col>
				<v-menu
					v-model="showWarrantyDatePicker"
					:close-on-content-click="false"
					:nudge-right="40"
					transition="scale-transition"
					offset-y
					min-width="290px"
				>
					<template v-slot:activator="{ on }">
					<v-text-field
						v-model="warrantyPurchaseDate"
						label="Purchase date"
						prepend-icon="event"
						readonly
						v-on="on"
						clearable
					></v-text-field>
					</template>
					<v-date-picker v-model="warrantyPurchaseDate" @input="showWarrantyDatePicker = false"></v-date-picker>
				</v-menu>
			</v-col>
			</v-row>
			</v-card-text>
		</v-card>
		</validation-observer>
	</div>
</template>
<script lang="ts">
import CreateOrEditMoneyActivity from "./CreateOrEditMoneyActivityModel";
export default CreateOrEditMoneyActivity;
</script>