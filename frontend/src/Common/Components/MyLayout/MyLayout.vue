<template>
    <div>
        <v-navigation-drawer
            v-model="drawer"
            app
            clipped
            fixed
        >
            <v-list>
            <v-list-item>
                <v-list-item-avatar>
                    <v-img src="">image here</v-img>
                </v-list-item-avatar>
            </v-list-item>

            <v-list-item link>
                <v-list-item-content>
                    <v-list-item-title class="title">
                        John Doe
                    </v-list-item-title>
                    <v-list-item-subtitle>name@domain.com</v-list-item-subtitle>
                </v-list-item-content>
            </v-list-item>
        </v-list>
        <v-divider></v-divider>
        <v-list
            nav
            dense
        >
            <v-list-item-group
                v-model="selectedItem"
                color="primary"
            >
                <v-list-item
                    v-for="(item, index) in navigationItems"
                    :key="index"
                    @click="navigate(item.route)"
                >
                    <v-list-item-icon>
                        <v-icon v-text="item.icon"></v-icon>
                    </v-list-item-icon>

                    <v-list-item-content>
                        <v-list-item-title v-text="item.name"></v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
            </v-list-item-group>
        </v-list>
        </v-navigation-drawer>
        <v-app-bar
            app
            clipped-left
        >
        <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
        <v-toolbar-title>MyFinances</v-toolbar-title>
        </v-app-bar>
    </div>
</template>
<script lang="ts">
import MyLayout from "./MyLayoutModel";
export default MyLayout;
</script>
