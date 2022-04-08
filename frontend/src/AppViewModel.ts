import MyLayout from "./Common/Components/MyLayout/MyLayout.vue";

import Vue from "vue";
import Component from "vue-class-component";

@Component({
    name: "AppViewModel",
    components: {
        MyLayout
    }
})
export default class App extends Vue {
}
