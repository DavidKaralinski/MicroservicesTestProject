import Searchbar from './Searchbar'
import Logo from './Logo'
import LoginButton from './LoginButton'
import { getCurrentUser } from '../../actions/authActions'
import UserActions from './UserActions'

export const NavBar = async () => {
    const user = await getCurrentUser();

    return (
        <header className='sticky top-0 z-50 flex justify-between shadow-md bg-white items-center p-5'>
            <Logo />
            <Searchbar />
            {!!user ? <UserActions user={user} /> : <LoginButton />}
        </header>
    )
}