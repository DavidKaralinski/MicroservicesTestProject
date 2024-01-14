import React from 'react'
import { getCurrentSession } from '../../actions/authActions'
import Heading from '../components/Heading';
import AuthTest from './AuthTest';

export default async function SessionData() {
    const session = await getCurrentSession();

  return (
    <div>
        <Heading title='Current session data' />

        <div className='bg-blue-300 border-2 border-blue-600'>
            <h3 className='text-lg'>Data</h3>
            <pre>{JSON.stringify(session)}</pre>
        </div>

        <div className='mt-4'>
          <AuthTest />
        </div>
    </div>
  )
}
