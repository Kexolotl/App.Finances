<template>
	<div v-if="model">
		<validation-observer ref="defaultObserver">
		<v-card v-if="model">
			<v-toolbar dark color="primary">
			<v-btn icon dark @click="back()">
				<v-icon>mdi-arrow-left</v-icon>
			</v-btn>
			<v-toolbar-title>Standing order</v-toolbar-title>
			<v-spacer></v-spacer>
				<v-btn icon dark @click="saveAsync()">
					<v-icon>mdi-content-save</v-icon>
				</v-btn>
			</v-toolbar>
			<v-card-text>
				<v-row no-gutters>
					<v-col cols="6">
						<validation-provider rules="required" name="Amount" v-slot="{ errors }">
							<v-text-field
								:error-messages="errors[0]"
								prepend-icon="mdi-cash"
								label="Amount"
								v-model="model.amount"
							></v-text-field>
						</validation-provider>
					</v-col>
					<v-col cols="6" class="pl-1">
						<validation-provider rules="required" name="First payment date" v-slot="{ errors }">
							<v-menu
								v-model="showFirstPaymentDatePicker"
								:close-on-content-click="false"
								:nudge-right="40"
								transition="scale-transition"
								offset-y
								:disabled="!isNew"
								min-width="290px">
								<template v-slot:activator="{ on }">
								<v-text-field
									:error-messages="errors[0]"
									v-model="firstPaymentDate"
									label="First payment date"
									prepend-icon="event"
									readonly
									v-on="on"
									:disabled="!isNew"
								></v-text-field>
								</template>
								<v-date-picker v-model="firstPaymentDate" @input="showFirstPaymentDatePicker = false"></v-date-picker>
							</v-menu>
						</validation-provider>
					</v-col>
					<v-col cols="6" class="pl-1">
						<v-menu
							v-model="showFinalPaymentDatePicker"
							:close-on-content-click="false"
							:nudge-right="40"
							transition="scale-transition"
							offset-y
							min-width="290px">
							<template v-slot:activator="{ on }">
							<v-text-field
								v-model="finalPaymentDate"
								label="Final payment date"
								prepend-icon="event"
								readonly
								v-on="on"
								clearable
							></v-text-field>
							</template>
							<v-date-picker v-model="finalPaymentDate" @input="showFinalPaymentDatePicker = false"></v-date-picker>
						</v-menu>
					</v-col>
					<v-col cols="6">
						<validation-provider rules="required" name="Category" v-slot="{ errors }">
							<v-autocomplete
								prepend-icon="mdi-package"
								:error-messages="errors[0]"
								label="Category"
								item-text="name"
								return-object
								v-model="model.category"
								:items="model.categories"
								clearable
							></v-autocomplete>
						</validation-provider>
					</v-col>
					<v-col cols="6" class="pl-1">
						<v-autocomplete
							prepend-icon="mdi-office-building"
							label="Business"
							item-text="name"
							v-model="model.business"
							:items="model.businesses"
							return-object
							clearable
							:disabled="!isNew"
						></v-autocomplete>
					</v-col>

					<v-col cols="6">
						<v-select
							prepend-icon="mdi-credit-card"
							label="Payment type"
							v-model="model.paymentType"
							:items="paymentTypeItems"
							item-value="id"
							item-text="name"
							:disabled="!isNew"
						></v-select>
					</v-col>
					<v-col cols="6" class="pl-1">
						<v-select
							prepend-icon="local_activity"
							label="Activity type"
							v-model="model.activityType"
							:items="activityTypeItems"
							item-value="id"
							item-text="name"
							@change="changeActivityType()"
							:disabled="!isNew"
						></v-select>
					</v-col>
					<v-col cols="6">
						<v-select
							prepend-icon="mdi-repeat"
							label="Frequency"
							v-model="model.frequency"
							:items="frequencyTypeItems"
							item-value="id"
							item-text="name"
							:disabled="!isNew"
						></v-select>
					</v-col>
					<v-col cols="6" class="pl-1">
						<v-checkbox 
							v-model="model.importantForTax" 
							label="Important for tax"
							:disabled="!isNew"
						></v-checkbox>
					</v-col>
					<v-col cols="6">
						<v-switch
							v-show="!isNew"
							v-model="model.isActive"
							label="Is active"
						></v-switch>
					</v-col>
				</v-row>
			</v-card-text>
		</v-card>
		</validation-observer>
	</div>
</template>
<script lang="ts">
import CreateOrEditStandingOrder from "./CreateOrEditStandingOrderModel";
export default CreateOrEditStandingOrder;
</script>