<template>
  <div id="app">
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <!-- Brand and toggle get grouped for better mobile display -->
        <div class="navbar-header">
          <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#iti-navbar-collapse" aria-expanded="false">
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
      </button>
          <a class="navbar-brand" href="#">ITI.PrimarySchool</a>
        </div>

        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class="collapse navbar-collapse" id="iti-navbar-collapse">
          <ul class="nav navbar-nav">
            <li><a href="#">Gestion des classes </a></li>
            <li><a href="#">Gestion des élèves</a></li>
            <li><a href="#">Gestion des professeurs</a></li>
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li class="dropdown">
              <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">{{ email }} <span class="caret"></span></a>
              <ul class="dropdown-menu">
                <li><a href="#">Se déconnecter</a></li>
              </ul>
            </li>
          </ul>
        </div>
        <!-- /.navbar-collapse -->
      </div>
      <!-- /.container-fluid -->

      <div class="progress" v-show="isLoading">
        <div class="progress-bar progress-bar-striped active" role="progressbar" style="width: 100%"></div>
      </div>
    </nav>

  </div>
</template>

<script>

import $ from 'jquery'
import { mapGetters, mapActions } from 'vuex'

export default {
  data () {
    return {

    }
  },

  computed: {
    email: () => '',
    ...mapGetters(['isLoading'])
  }
}
</script>

<style lang="less" scoped>
  .progress {
    margin: 0px;
    padding: 0px;
    height: 5px;
  }
</style>

<style lang="less">
  @import "../styles/global.less";
</style>