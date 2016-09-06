<template>
  <div id="app">
    <img src="../assets/logo.png">
    <h1>{{ msg }}</h1>
    <h2>Count:  {{ count }}/10</h2>

    <increment-button></increment-button>
  </div>
</template>

<script>
import IncrementButton from './IncrementButton.vue'

export default {
  components: { IncrementButton },
  
  data () {
    return {
      msg: 'Hello Vue!'
    }
  },

  computed: {
    count() {
      return this.$store.getters.count
    }  
  }
}
</script>

<style lang="less">
@import "../styles/global.less";

body {
  font-family: Helvetica, sans-serif;
}
</style>
