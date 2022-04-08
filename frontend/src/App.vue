<template>
	<v-app id="myFinances">
		<confirmation-dialog ref="confirmationDialog"></confirmation-dialog>

		<my-layout ref="myLayout"></my-layout>
		<v-main dark>
			<router-view/>
		</v-main>
	</v-app>
</template>
<script lang="ts">
import App from "./AppViewModel";
export default App;
</script>
<style lang="scss">
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
#nav {
  padding: 30px;
  a {
	font-weight: bold;
	color: #2c3e50;
	&.router-link-exact-active {
	  color: #42b983;
	}
  }
}
</style>
