<template>
  <div id="app">
    <img src="./assets/logo.png">
    <h1>{{ msg }}</h1>

    <button type="button" class="btn btn-primary">A button with Bootstrap style!</button>
  </div>
</template>

<script>
export default {
  data () {
    return {
      msg: 'Hello Vue!'
    }
  }
}
</script>

<style lang="less">
@import "./styles/global.less";

body {
  font-family: Helvetica, sans-serif;
}
</style>
