import React from 'react';
import PropTypes from 'prop-types';

function LanguageSwitcher(props) {

    function handleLanguageChange(e) {

        if (typeof window !== 'undefined' && e && e.target) {

            window.location.href = e.target.value;
        }
    }

    const languages = props.availableLanguages.map(language => <option key={language.uniqueId} value={language.url} selected={language.uniqueId === props.selectedLanguageUniqueId}>{language.text}</option> );

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
    selectedLanguageUniqueId: PropTypes.string.isRequired
};

export default LanguageSwitcher;