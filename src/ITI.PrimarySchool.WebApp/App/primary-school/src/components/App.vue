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
          <router-link class="navbar-brand" to="/">ITI.PrimarySchool</router-link>
        </div>

        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class="collapse navbar-collapse" id="iti-navbar-collapse" v-if="auth.isConnected">
          <ul class="nav navbar-nav">
            <li><router-link to="/classes">Gestion des classes</router-link></li>
            <li><router-link to="/students">Gestion des élèves</router-link></li>
            <li><router-link to="/teachers">Gestion des professeurs</router-link></li>
            <li v-required-providers="['GitHub']"><router-link to="/github/following">Elèves suivis sur GitHub</router-link></li>
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li class="dropdown">
              <a href="#" v-bs-dropdown class="dropdown-toggle">{{ auth.email }} <span class="caret"></span></a>
              <ul class="dropdown-menu">
                <li><router-link to="/logout">Se déconnecter</router-link></li>
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

    <div class="container">
      <router-view class="child"></router-view>
    </div>

  </div>
</template>

<script>

import AuthService from '../services/AuthService'
import { mapGetters, mapActions } from 'vuex'
import '../directives/requiredProviders'
import '../directives/bsDropdown'

export default {
  computed: {
    ...mapGetters(['isLoading']),    
    auth: () => AuthService
  }
}
</script>

<style lang="less" scoped>
  .progress {
    margin: 0px;
    padding: 0px;
    height: 5px;
  }

  a.router-link-active {
    font-weight: bold;
  }

</style>

<style lang="less">
  @import "../styles/global.less";
</style>