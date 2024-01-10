import Searchbar from './Searchbar'
import Logo from './Logo'

export const NavBar = () => {
    return (
        <header className='sticky top-0 z-50 flex justify-between shadow-md bg-white items-center p-5'>
            <Logo />
            <Searchbar />
            <div>Login</div>
        </header>
    )
}