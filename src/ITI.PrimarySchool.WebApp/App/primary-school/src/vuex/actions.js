import sampleApi from '../api/sample-api'
import * as types from './mutation-types'

export const increment = ({commit}) => {
    commit(types.INCREMENT)
}