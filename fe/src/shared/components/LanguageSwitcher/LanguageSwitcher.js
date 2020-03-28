import React from 'react';
import PropTypes from 'prop-types';

function LanguageSwitcher(props) {

    function handleLanguageChange(e) {

        if (typeof window !== 'undefined' && e && e.target) {

            window.location.href = e.target.value;
        }
    }

    const languages = props.availableLanguages.map((language, index) => <option key={index} value={language.url} selected={language.text === props.selectedLanguageText}>{language.text}</option> );

    return (
        <div className="select">
            <select onChange={(e) => handleLanguageChange(e)}>
                {languages}
            </select>
        </div>
    );
}

LanguageSwitcher.propTypes = {
    availableLanguages: PropTypes.array.isRequired,
    selectedLanguageText: PropTypes.string.isRequired
};

export default LanguageSwitcher;